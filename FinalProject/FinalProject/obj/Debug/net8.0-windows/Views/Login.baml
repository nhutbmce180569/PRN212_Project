   M S B A M L   `   `   `  ����G  CFinalProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:FinalProjectFinalProject  5   	       FinalProject.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:FinalProject     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � 
ResizeMode$ NoResize=�65    � 	Resources 5	         	TextStyle �� Property �� Value  btnLogin �� 
TargetType �� 
TargetType �� CornerRadius��%�  &	        &	 �     7   �� 5
   
   .�5      ��  � ��
 �����6C   �� 5       � ��$ 13(�61   6>   �� 5       � ��$
 Bold"�63   6B   5      �� 5   
   " ��6!   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 10��6%   ���	#FF46BE126P   �� 5      8��  $��Center=�6R   $��Center=�6m   $��Login��6�   5      5      5      5      5      .��5      �� $ 6��6   
������� �� Padding$ 20q�5      $��376,69,78,69q�5       	 J� Effect	 5!   
   3  -System.Windows.Media.Effects.DropShadowEffect  5"      
   
BlurRadius$
 15Q�6      	Direction$	 270Q�5#   
      Opacity$	 0.5Q�5$   
      ShadowDepth$ 0Q�5%   
   ._�5'   
   � $��-20,-10,-20,-10q�6    � RowDefinitions 5(      �� 5)      $��* �6    6-   �� 5*      $��* �6    6-   �� 5+      $��* �6    6-   �� 5,      $��* �6    6-    � ColumnDefinitions 5.      �� 5/      $��* �6#   6/   �� 50      $��2* �6#   60   .+�53      �� $��0��6   $��2��6!   $��Login��65   $��Center=�6E   $��Center=�6`   $
��Bold"�6}   $��30(�6�   �� 54      $
��80px��6   $��	0 0 -20 0q�6!   $��Username��64   $��1��6G   $��0��6T   $��Center=�6d   $��Center=�6   $��15(�6�   �� 55      $	��5.5q�6   $��1.5pxq�6$   $��1��6<   $��1��6I   $��150px��6Y   $
��30px��6g   $��-10,13,47,13q�6u   �� 56      $
��80px��6   $��	0 0 -20 0q�6!   $��Password��64   $��2��6G   $��0��6T   $��Center=�6d   $��Center=�6   $��15(�6�   �� 57      $	��5.5q�6   $	��2pxq�6$   $��2��6:   $��1��6G   $��150px��6W   $
��30px��6e   $��-10,13,47,13q�6s   �� 59      -   5        #��[ 59      $��3��67   $��1��6D   $��20,10,81,20q�6T   ���	#FF1A684F6~   �� 5:      $
��Left=�6   $	��178��6/   $��-287,47,0,0q�6<   $��4��6Q   $	��Top=�6b   $	��204��6z   $��/Images/img-01.png��6�   5<      5>      5?      