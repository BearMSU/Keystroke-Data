   M S B A M L   `   `   `  ����B  >KeyTime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:KeyTimeKeyTime  5   	       KeyTime.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:KeyTime     5   	     � Title$  KeyTime��5   	   $	��450��6   $	��800��6&   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��105.882 �6   �� 5      $��479.118 �6   �� 5      $��0* �6   .+�5   
   �� -   5         ��Name$ textName��5      $
��Left=�6$   $��16��6?   $��
194,84,0,0q�6K    �� TextWrapping$
 Wrap=�6_   $	��Top=�6s   $	��225��6�   $��1��6�   �� 5   
   -   5         ��Name$ lblName��5      $��Name:��6!   $
��Left=�61   $��31��6L   $��
136,76,0,0q�6X   $	��Top=�6l   $��53��6�   $��1��6�   �� 5   
   -   5         ��Name$ btnNext��5      $
��Next��6"   $
��Left=�61   $��371,237,0,0q�6L   $	��Top=�6a   $��20��6�   $��48��6�   $��1��6�   �� 5   
   -   5        $ btnRedo��5      $
��Redo��6"   $
��Left=�61   $��20��6L   $��194,242,0,0q�6X   $	��Top=�6m   $��48��6�    J� RenderTransformOrigin$ -0.107,0.352&�6�   $��1��6�   �� 5   
   -   5         ��Name$ textBlockInstructions��5      $
��Left=�63   $��194,135,0,0q�6N   $
��Wrap=�6c   $	��Top=�6w   $��64��6�   $	��225��6�   ${�False��6�   4  .System.Windows.Automation.AutomationProperties   HelpText$$ Instructions for using KeyTime��6�   $���~Enter the word "Michigan" in the box below. Press Next or Redo.  After you have entered the results 10 times the test is over.��6�   $��1��6�  �� 5   
   -   5        $ textPassword��5      $
��Left=�6(   $��16��6C   $��194,204,0,0q�6O   $�� ��6d   $
 Wrap=�6l   $	��Top=�6�   $	��225��6�   	 J� IsManipulationEnabled$
	 True��6�   $��1��6�   �� 5   
   -   5        $ 
textResult��5      $
��Left=�6(   $��
20,280,0,0q�6C   $
��Wrap=�6W   $	��Top=�6k   $	��452��6�   $��2��6�   �� 5   
   -   5        $ btnTest��5      $
��Test��6"   $
��Left=�61   $��371,278,0,0q�6L   $	��Top=�6a   $y�Visible=�6y   $��1��6�   ${�False��6�   $��48��6�   $��20��6�   � 5   
   -	   5        
 �Name$
 
pbProgress��5      $��1��6*   $
��Left=�6:   $	��109��6U   $��439,111,0,0q�6b   $	��Top=�6w   $��13��6�    � Orientation$ Vertical=�6�   $y�Visible=�6�   5      5      