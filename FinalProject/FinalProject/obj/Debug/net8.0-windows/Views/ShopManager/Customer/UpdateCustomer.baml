   M S B A M L   `   `   `  ����G  CFinalProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:FinalProject.Views.ShopManager.Customer'FinalProject.Views.ShopManager.Customer  5   	   <    6FinalProject.Views.ShopManager.Customer.UpdateCustomer   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Glocal5clr-namespace:FinalProject.Views.ShopManager.Customer     5   	     � WindowStyle$
  None=�5       � AllowsTransparency$
 True��6    
������� � Topmost$
 True��6Q    � Title$ AddCustomer��5	   	   $	��350��6   $	��500��6*    � 	Resources 5
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
��6.5* �6#   62   .+�5/      �� $��2��6   $��Update Customer��6,   $
��Left=�6C   $
��Bold"�6^   $��22(�6p   6�   �� 50      $��Fullname��6   $��0��6(   $��1��68   6G   �� 51      $��Gender��6   $��0��6&   $��2��66   6E   �� 52      $��Email��6   $��0��6%   $��3��65   6D   �� 53      $��PhoneNumber��6   $��0��6+   $��4��6;   6J   �� 54      $��Birthday��6   $��0��6(   $��5��68   6G   �� 55      $��Password��6   $��0��6(   $��6��68   6G   �� 56      -   5         ��Name$ txtFullName��56      $��1��6+   $��1��6;   ��6H   �� *TextBoxItem.FullName+ �� Mode$ OneWayToSource=�6�   �� 57      -   5        $��	txtGender��57      $
��Left=�6(   $��1��6C   $��2��6S   $��30��58   D   $	��190��6P   ��57   `   �� *Gender+ �� SelectedItem 58      �� *TextBoxItem.Gender+$ TwoWay=�6^   �� 59      -   5        $ txtEmail��59      $��1��6(   $��3��68   ��6E   �� *TextBoxItem.Email+$ OneWayToSource=�6   �� 5:      -   5        $ txtPhoneNumber��5:      $��1��6.   $��4��6>   ��6K   �� *TextBoxItem.PhoneNumber+$ OneWayToSource=�6�   (  "System.Windows.Controls.DatePicker  5;      -   5          Name$ txtBirthday��5;      $��1��6.   $��5��6>   $��40��6K   $	��190��6W   $
��Left=�6c      SelectedDateFormat$ Short=�5=         SelectedDate 5<      �� *TextBoxItem.Birthday+$ OneWayToSource=�5>      �� 5?      -   5        $ txtPassword��5?      $��1��6+   $��6��6;   ��6H   �� *TextBoxItem.Password+$ OneWayToSource=�6�   � 5@      $��8��6   $��1��6    
 5A      �� 5B      $��* �6'   63   �� 5C      $��* �6'   63   �� 5D      $��* �6'   63   	 5F      �� 5G      $��* �6$   61   .+�5I      �� 
����  �
�������$��30��6>   $��2��6J   $��0��6Z   $��Update��6g   ��6x   �� *UpdateCommand+6�   �� 5J      -   5        ���	#FFEE4F4F
�������$��30��5J   C   $��1��6O   $��0��6_   $��Close��6l   6�   5K      5L      5M      5N      