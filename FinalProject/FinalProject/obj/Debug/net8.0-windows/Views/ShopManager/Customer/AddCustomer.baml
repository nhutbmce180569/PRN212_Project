   M S B A M L   `   `   `  ����G  CFinalProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:FinalProject.Views.ShopManager.Customer'FinalProject.Views.ShopManager.Customer  5   	   9    3FinalProject.Views.ShopManager.Customer.AddCustomer   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Glocal5clr-namespace:FinalProject.Views.ShopManager.Customer     5   	     � WindowStyle$
  None=�5       � AllowsTransparency$
 True��6    
������� � Topmost$
 True��6Q    � Title$ AddCustomer��5	   	   $	��400��6   $	��550��6*    � 	Resources 5
       �� 
TargetType �� Property �� Value��%O  '��       '�� �     '�� �    �� 5   
   " ��6   .�5      ��  � ��$ Right=�64   6D   �� 5       � ��$ Center=�62   6C   �� 5       �@��Padding$
 10pxq�6(   67   5      �� 5   
   " ��6   .�5      ��  � ��$	 5pxq�6(   66   �� 5       � ��$ 30��6'   64   �� 5       � ��$	 190��6&   64   �� 5       �@��TextWrapping$ NoWrap=�6-   6>   �� 5       � ��$
 Left=�64   6C   5      �� 5   
   " ��6   .�5      ��  � ��$
 40px��6'   66   �� 5       � ��$ 100px��6&   66   5      5      .��5      ��  �� CornerRadius$ 20��6   ._�5   
   � -   5        	 � RowDefinitions	 5       �� 5!      $��* �6    6-   �� 5"      $��* �6    6-   �� 5#      $��* �6    6-   �� 5$      $��* �6    6-   �� 5%      $��* �6    6-   �� 5&      $��* �6    6-   �� 5'      $��* �6    6-   �� 5(      $��* �6    6-   �� 5)      $��* �6    6-   
 � ColumnDefinitions
 5+      �� 5,      $
��3.5* �6#   62   �� 5-      $
��6.5* �6#   62   .+�5/      �� $��2��6   $��Add Customer��6,   $
��Left=�6@   $
��Bold"�6[   $��22(�6m   6}   �� 50      $��Fullname��6   $��0��6(   $��1��68   6G   �� 51      $��Gender��6   $��0��6&   $��2��66   6E   �� 52      $��Email��6   $��0��6%   $��3��65   6D   �� 53      $��PhoneNumber��6   $��0��6+   $��4��6;   6J   �� 54      $��Birthday��6   $��0��6(   $��5��68   6G   �� 55      $��Password��6   $��0��6(   $��6��68   6G   �� 56      $��1��6   $��1��6&   ��63   �� *TextBoxItem.FullName+ �� Mode$ TwoWay=�6h   �� 58      -   5         ��Name$ cbGender��58   O   $
��Left=�6   $��1��62   $��2��6B   
�������$��30��59   P   $	��190��6\    �� SelectedItem 6   �� *TextBoxItem.Gender+$ TwoWay=�6j   �� 5:      $��1��6   $��3��6&   ��63   �� *TextBoxItem.Email+$ TwoWay=�6e   �� 5;      $��1��6   $��4��6&   ��63   �� *TextBoxItem.PhoneNumber+$ TwoWay=�6k   (  "System.Windows.Controls.DatePicker  5<      $��1��6   $��5��6)   $��40��66   $	��190��6B   $
��Left=�6N      SelectedDateFormat$ Short=�5>   
      SelectedDate 5=   
   �� *TextBoxItem.Birthday+$ TwoWay=�5?      �� 5@      $��1��6   $��6��6&   ��63   �� *TextBoxItem.Password+$ TwoWay=�6h   � 5A      $��8��6   $��1��6    
 5B      �� 5C      $��* �6'   63   �� 5D      $��* �6'   63   �� 5E      $��* �6'   63   	 5G      �� 5H      $��* �6$   61   .+�5J      �� 
����  �
�������$��30��6>   $��2��6J   $��0��6Z   $	��Add��6g   ��6u   �� *
AddCommand+6�   �� 5K      -   5        ���	#FFEE4F4F
�������$��30��5K   C   $��1��6O   $��0��6_   $��Close��6l   6�   5L      5M      5N      5O      