   M S B A M L   `   `   `  ����G  CFinalProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:FinalProject.Views.ShopManagerFinalProject.Views.ShopManager  5   	   4    .FinalProject.Views.ShopManager.ShopManagerView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   >local,clr-namespace:FinalProject.Views.ShopManager     5   	   c _MaterialDesignThemes.MahApps, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a_ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5   	     � Title$  ShopManagerView��5	   	   $	��600��6!   $
��1000��6.    � 	Resources 5
         ScrollThumbs �� 
TargetType �� Property �� Value �� 
TargetType �Name ��Name �� CornerRadius	 �� Triggers
 P� Property P� Value �� 
TargetName ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton �� Angle�� 5   
   %   &	        '�� D    �� 5      " n�6)   .�5      ��  � �� 5      �� 5      " n�6*   .R�5      � $
 Grid��6#   .+�5   "   �� $��Stretch=�6,   $��Stretch=�6J   $
��Auto��6f   $
��Auto��6s   
������ 6�   �� 5   "   $ 
Rectangle1��6)   $ 10��6=   $��Stretch=�6O   $��Stretch=�6m   $
��Auto��6�   $
��Auto��6�   #��z��6�   5      	 5      P� 5   "   
 �@�Tag$ 
Horizontal��69   . �5   &   �� $ 
Rectangle1��6-    � ��$
 Auto��6V   �� 5   &   $ 
Rectangle1��6-    � ��$ 7��6W   5   #   5      5      5      �� 5!      " ��6/   .�5"      ��  �@��IsFlicksEnabled$ false��6;   �� 5#       � ��
 �����6/   �� 5$       � ��
 �����6/   �� 5%       � ��$ 10��6*   �� 5&       � �� 5'      �� 5(      " ��6*   .R�5)      � $ GridRoot��6#   $��19��65   ��6@   Z� .+�5*   "   ]� $ 
PART_Track��6(   $��0��6<   $
��true��6I   $|�false��6d    5+   &   n� 5,   *   $ Thumb��60   #��z��6?   #���   6i    5.   &   �� 5/   *   $ PageUp��67   $��ScrollBar.PageDownCommand��6G   $ 0Q�6k   $|�false��6w    51   &   �� 52   *   $ PageDown��67   $��ScrollBar.PageUpCommand��6I   $ 0Q�6k   $|�false��6w   54   #   55      	 57      P� 59   "   
 � {�$ false��6?   . �5:   &   �� $ Thumb��6-    � y�$ 	Collapsed=�6V   5;   #   P� 5<   "   
 �@��Orientation$ 
Horizontal=�6A   . �5=   &   �� $ GridRoot��6-    �@�LayoutTransform 5>   *   �� 5?   .   $	 -90Q�6>   5A   '   �� 5B   &   $ 
PART_Track��6-    �@�LayoutTransform 5C   *   �� 5D   .   $	 -90Q�6>   5F   '   �� 5G   &    � ��$
 Auto��6>   �� 5H   &    � ��$ 12��6?   �� 5I   &   $ Thumb��6-    �@�Tag$ 
Horizontal��6O   �� 5J   &   $ PageDown��6-    � ��$ ScrollBar.PageLeftCommand��6V   �� 5K   &   $ PageUp��6-    � ��$  ScrollBar.PageRightCommand��6T   5L   #   5N      5P      5Q      5R      .��5V      �  � ColumnDefinitions 5W   
   �� 5X      $	��200 �6   �� 5Y      $��* �6    � RowDefinitions 5[   
   �� 5\      $��35 �6   �� 5]      $��* �6   .+�5a   
   � $��1��6   $��1��6   ���	#FFCFCFCF.+�5b       � -   5          �Name$	 fmt��5b        � NavigationUIVisibility$ Hidden=�6!   6C   5c      � 5e   
   $��1��6   ��5f      �� 5g       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5h      �  � Color$ #42A5F5��6#    � Offset$ 1Q�63   � 5i      $ 	#FFE6E6E6��6#   5j       J� Effect 5m      3  -System.Windows.Media.Effects.DropShadowEffect  5n         
BlurRadius$ 20Q�6#      Color$ 	#FFDEDEDE��63       RenderingBias$  Quality=�6E   !   ShadowDepth$! 1Q�6]   .+�5s      �� $��1��6   .+�5t      �� -   5        $��10q�5t      
����������#1565C0���#1565C0$��30��6�   .��5u      � $	��150��6   .+�5v      �� $��Center=�6$   $��	Customers��6A   $��Champagne & Limousines)�6R   $��14��6z   $��Center=�6�   5w      5x      �� 5z      $��10q�6   
����������#1565C0���#1565C0$��30��6c   .��5{      � $	��150��6   .+�5|      �� $��Center=�6$   $��Products��6A   $��Champagne & Limousines)�6Q   5}      5~      �� 5      $��10q�6   ���#D32F2F
������ 
������ .��5�      � $	��150��6   .+�5�      �� $��Center=�6$   $��Logout��6A   $��Champagne & Limousines)�6O   5�      5�      5�      5�      � 5�   
   -   5        $ GridBarraTitulo��5�      $��2��6(   ���#1565C0 5�      �� 5�      $
��2.5* �6#   63   �� 5�      $
��7.5* �6#   63   .+�5�      � $��0��6   $��0��6    .+�5�      �� $��10,0,0,0q�6   $ ��/Images/Group 1521 (3).png��6*   $��120px��6N   $��30��6\   $
��Left=�6h   5�      �� 5�      $��1��6   " �� Orientation$" 
Horizontal=�6)   $��Right=�6B   $
��10,0q�6^   .+�5�      �� $��25��6   $��25��6$   
���������60   Z� ��6F   Z� .��5�      '  !MaterialDesignThemes.Wpf.PackIcon  #   Kind$# Account=�6.   5�      �� 5�      -   5        $ ��Name$$ ButtonFechar��5�      $��25��6/   $��25��6:   
���������6F   Z� ��6\   Z� .��5�        $# Power=�6.   5�      5�      5�      5�      5�      