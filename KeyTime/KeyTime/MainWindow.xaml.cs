using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace KeyTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //working constants
        private const int MAX_CLICKS = 10;    //number of trials
        private const string NEXT = "Next";  //button labels.
        private const string FINI = "Finish";
        private const string COLLECT_INSTRUCT = "Enter the word &quot;Michigan&quot; in the box below. Press Next or Redo.  After you have entered the results 10 times data collection is over.";
        private const string RECOGNITION_INSTRUCTION = "Enter the word &quotMichigan&quot in the box below. Click Enter when done. You will be told if you are recognized.";
        private const string SUCCESS_MSG = "I'd know that typist any where!";
        private const string FAIL_MSG = "Who you trying to fool? -- Not Recognized";
        private const int SUCCESS_LEVEL = 80;
        private const double Z_MULTIPLIER = 1.833;  //68% confidence level =1,     90% confidence level = 1.645, 95% confidence level = 1.96 (assumes normal distribution)
                                                    //80% confidence level =1.383, 90% confidence level = 1.833, 95% confidence level = 2.262 (t-distribution with 9 degrees of freedom)

        //these two constants need to be changed in tandom!
        public const string THE_WORD = "MICHIGAN";  //magic word user is asked to enter(all caps to allow direct compare to e.Key)
        public const int WORD_LENGTH = 8;           //length of magic word

        //working variables
        private int clickCnt = 0;    //counts # of trials completed (typed THE_WORD and pressed next
        private int keypressCnt = 0; //within a trial counts number of keystrokes recorded
        private string csvData;     //json formatted output text
        private int[,] times = new int[MAX_CLICKS, WORD_LENGTH]; //collects individual timing events
        private DateTime dwnTime,    //holds time that the key down event was processed
                          upTime;    //holds time that the key up event was processed.
        private int[,] avgLetter = new int[WORD_LENGTH, 3]; //letterAvg[0,0] is total key time for first letter keystrokes--letterAvg[0,1] is the number of A keystrokes included in total
        private int[] stdLetter = new int[WORD_LENGTH];
        private string stdLine,
                       avgLine;

        private bool isTestOn = false;

        public MainWindow()
        {
            InitializeComponent();

            textPassword.KeyDown += TextPassword_KeyDown;
            textPassword.KeyUp += TextPassword_KeyUp;
            textName.Focus();
            for (int i = 0; i < WORD_LENGTH; i++)
            {
                avgLetter[i, 0] = 0;  //time total
                avgLetter[i, 1] = 0;  //number of clicks
                avgLetter[i, 2] = 0;  //key average ms
            }
        }

        private int stdDev(int[] rawData)
        {
            int result = -1;
            int total = 0;
            int avg;
            int count = rawData.Length;

            if (count > 2)
            { //calculate StdDev
                for (int i = 0; i < count; i++)
                    total += rawData[i];
                avg = total / count;
                total = 0;
                for (int j = 0; j < count; j++)
                {
                    total += (rawData[j] - avg) * (rawData[j] - avg); //sum squared difference from Average
                }
                result = (int)Math.Sqrt(total / count);
            }
            return result;
        }

        private void dispData()
        {
            int[] total = new int[MAX_CLICKS];
            int[] average = new int[MAX_CLICKS + 1];  //column = average for that key-- extra column for average of word strokes
            int[] raw = new int[WORD_LENGTH];
            int[] rawLetter = new int[MAX_CLICKS];
            int[] stdev = new int[MAX_CLICKS + 1];
            int trialCnt = 1;

            //display Headers
            textResult.Text = "          ";
            for (int h = 0; h < WORD_LENGTH; h++)
            {
                textResult.Text += THE_WORD[h] + "      ";
            }
            textResult.Text += " Average      StdDev\n";

            //display by trial
            for (int i = 0; i < MAX_CLICKS; i++, trialCnt++)
            {
                textResult.Text += i.ToString() + "       ";
                total[i] = 0;
                for (int j = 0; j < WORD_LENGTH; j++)
                {
                    raw[j] = times[i, j];
                    total[i] += raw[j];
                    textResult.Text += raw[j].ToString() + "   ";
                }
                average[i] = total[i] / WORD_LENGTH;
                stdev[i] = stdDev(raw);
                textResult.Text += average[i].ToString() + " +/- " + stdev[i].ToString() + "\n";
            }
            //display letter avg
            avgLine = "Avg        ";
            stdLine = "Std        ";
            for (int k = 0; k < WORD_LENGTH; k++)
            {
                for (int l = 0; l < MAX_CLICKS; l++)
                {
                    rawLetter[l] = times[l, k];
                }
                stdLetter[k] = stdDev(rawLetter);
                avgLine += avgLetter[k, 2].ToString() + "    ";
                stdLine += stdLetter[k].ToString() + "     ";
            }
            textResult.Text += avgLine + "\n";
            textResult.Text += stdLine;
        }

        private void saveData()
        {
            string path = Directory.GetCurrentDirectory();
            var fileName = @path + "\\KeyTime" + textName.Text + ".csv";
            int[] total = new int[MAX_CLICKS];
            int[] average = new int[MAX_CLICKS + 1];  //column = average for that key-- extra column for average of word strokes
            int[] raw = new int[WORD_LENGTH];
            int[] rawLetter = new int[MAX_CLICKS];
            int[] stdev = new int[MAX_CLICKS + 1];
            int trialCnt = 1;

            using FileStream fs = File.Create(fileName);
            using var sr = new StreamWriter(fs);
            //build file output
            for (int q = 0; q < MAX_CLICKS; q++)
            {
                total[q] = 0;
                average[q] = 0;
            }
            csvData = "\"" + textName.Text + "\",M,I,C,H,I,G,A,N,Average,StdDev\n";
            for (int i = 0; i < MAX_CLICKS; i++, trialCnt++)
            {
                csvData += "\"TRIAL" + trialCnt.ToString() + "\",";
                for (int j = 0; j < WORD_LENGTH; j++)
                {
                    csvData += times[i, j].ToString() + ",";
                    raw[j] = times[i, j];
                    total[i] += times[i, j];
                }
                average[i] = total[i] / WORD_LENGTH;
                stdev[i] = stdDev(raw);
                csvData += average[i].ToString();
                csvData += "," + stdev[i].ToString() + "\n";
            }
            average[MAX_CLICKS] = 0;
            for (int r = 0; r < MAX_CLICKS; r++)
            {
                //textResult.Text += average[r].ToString() + "  ";
                average[MAX_CLICKS] += average[r];
            }
            average[MAX_CLICKS] /= MAX_CLICKS;
            stdev[MAX_CLICKS] = stdDev(average);
            //textResult.Text += average[MAX_CLICKS].ToString() + " " + stdev[MAX_CLICKS].ToString();
            sr.WriteLine(csvData);
            sr.Flush();
            sr.Close();
        }

        private void TextPassword_KeyUp(object sender, KeyEventArgs e)
        {
            int upMSec;
            upTime = DateTime.Now;
            if (clickCnt < MAX_CLICKS && keypressCnt < WORD_LENGTH)
            {
                upMSec = upTime.Millisecond;
                if (upTime.Second != dwnTime.Second)
                {
                    upMSec += 1000; //correct for second rollover
                }
                times[clickCnt, keypressCnt] = upMSec - dwnTime.Millisecond;

                if (isTestOn)
                {
                    int val = times[clickCnt, keypressCnt];
                    int spread = (int)Math.Round(Z_MULTIPLIER * stdLetter[keypressCnt]);
                    //capture and test keystroke time
                    textResult.Text += "    " + val.ToString();
                    if (val >= (avgLetter[keypressCnt, 2] - spread) && val <= (avgLetter[keypressCnt, 2] + spread))
                    {
                        pbProgress.Value += 100 / WORD_LENGTH;
                    }
                }
                else
                {
                    avgLetter[keypressCnt, 0] += times[clickCnt, keypressCnt];
                    avgLetter[keypressCnt, 1]++;
                    if (avgLetter[keypressCnt, 1] != 0)
                    {
                        avgLetter[keypressCnt, 2] = avgLetter[keypressCnt, 0] / avgLetter[keypressCnt, 1];
                    }
                }
                keypressCnt++;
            }
        }

        private void TextPassword_KeyDown(object sender, KeyEventArgs e)
        {

            dwnTime = DateTime.Now;
            if (e.Key.ToString()[0] != THE_WORD[keypressCnt])
            {   //eliminate mistyping by forcing a redo...
                e.Handled = true;
                textPassword.Text = "";
                keypressCnt = 0;
                textPassword.Focus();
            }
        }

        private void BtnRedo_Click(object sender, RoutedEventArgs e)
        {
            textPassword.Text = "";
            keypressCnt = 0;
            textPassword.Focus();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //TODO: clear/disable user entry points except textPassword

            if (btnTest.Content.Equals("Test"))
            {
                btnTest.Content = "Enter";
                //      change instructions
                textBlockInstructions.Text = RECOGNITION_INSTRUCTION;
                textPassword.Text = "";
                textPassword.Focus();
                keypressCnt = 0;
                isTestOn = true;
                textResult.Text += "\n";
                pbProgress.Value = 0;
            }
            else
            {
                //      success if times for 80% of letters within +/- 1Std of letter time
                if (pbProgress.Value > SUCCESS_LEVEL)
                {
                    textBlockInstructions.Text = SUCCESS_MSG;
                }
                else
                {
                    textBlockInstructions.Text = FAIL_MSG;
                }
                keypressCnt = 0;
                clickCnt = 0;
                btnTest.Content = "Test";
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            //clear textPassword textbox
            pbProgress.Value += 10;
            textPassword.Text = "";
            textPassword.Focus();
            if (!isTestOn)
            {
                clickCnt++;
            }
            if (clickCnt == 1)
            {
                textName.IsEnabled = false;
                btnTest.IsEnabled = false;
            }
            else if (clickCnt >= MAX_CLICKS)
            {
                //save test data
                dispData();
                saveData();
                btnNext.Content = NEXT;
                btnNext.IsEnabled = false;
                btnRedo.IsEnabled = false;
                clickCnt = 0;
                btnTest.IsEnabled = true;
                pbProgress.Value = 0;
            }
            else if (clickCnt == (MAX_CLICKS - 1))
            {
                btnNext.Content = FINI;
            }
            keypressCnt = 0;
        }

    }

}
