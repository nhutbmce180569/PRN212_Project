   M S B A M L   `   `   `  ����G  CFinalProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:FinalProject.Views.ShopManagerFinalProject.Views.ShopManager  5      5    /FinalProject.Views.ShopManager.CustomerListView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:FinalProject.Views.ShopManager     5        N� Title$  CustomerListView��5	       � 	Resources 5       �� 
TargetType �� Property �� Value)  #System.Windows.Controls.DataGridRow��%�  '��       '�� �     '�� �    '  �    �� 5   
   " ��6   .�5      ��  � ��$ Right=�64   6D   �� 5       � ��$ Center=�62   6C   �� 5       �@��Padding$
 10pxq�6(   67   5      �� 5   
   " ��6   .�5      ��  � ��$	 5pxq�6(   66   �� 5       � ��$ 30��6'   64   �� 5       � ��$	 190��6&   64   �� 5       �@��TextWrapping$ NoWrap=�6-   6>   �� 5       � ��$
 Left=�64   6C   5      �� 5   
   " ��6   .�5      ��  � ��$
 30px��6'   66   �� 5       � ��$
 80px��6&   65   �� 5       � �� �#1565C06=   �� 5       � ��
 �����6;   5      �� 5   
   "  6   .�5      ��  � ��$ 2q�6'   63   5       5!      .��5"      � ���#E3F2FD � RowDefinitions 5#   
   �� 5$      $��1* �6   6*   �� 5%      $��8* �6   6*   �� 5&      $��* �6   6)   .+�5(   
   �  � ColumnDefinitions 5)      �� 5*      $
��0.5* �6#   62   �� 5+      $��3* �6#   60   �� 5,      $
��6.5* �6#   62   .+�5.      �� $��1��6   $��Customer��6(   $
��Left=�68   $
��Bold"�6S   $��15(�6e   6u   �� 5/      $��0��6   $
��Left=�6L   $
��Bold"�6g   $��15(�6y   ��6(   �� *CustomerList.Count+6�   � 50      $��2��6    51      �� 52      $��7* �6'   64   �� 53      $
��1.5* �6'   66   .+�55      �� -   5         ��Name$ 	searchBox��55   j   $��Right=�6   ��66   �� *SearchBoxItem+ �� Mode$ OneWayToSource=�6   �� 56      $��1��6   $��Search��6)   ��6:   �� *SearchCommand+6^   57      58      � 59   
   $��2��6    5:      �� 5;      $��* �6#   6/   �� 5<      $��* �6#   6/   �� 5=      $��5* �6#   60   .+�5?      �� $��0��6   $��0��6%   $	��Add��62   ��6A   �� *OpenAddPopupCommand+6k   �� 5@      $��1��6   $��0��6%   $��Update��62   ��6C   �� *OpenUpdatePopupCommand+6p   �� 5A      $��2��6   $��0��6%   $��Delete��62   $��Right=�6e   $��0, 0, 10, 0q�6�   ���	#FFA10404��6C   �� *DeleteCommand+6�   5C      &   System.Windows.Controls.DataGrid  5D   
   $��1��6   	   AutoGenerateColumns$	 False��6    $��Hidden=�6�   ���#E3F2FD��6<   �� *CustomerList+
 �� SelectedItem
 6a   �� *SelectedItem+$ OneWayToSource=�   Columns 5E      0  *System.Windows.Controls.DataGridTextColumn  5F      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.4* 61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *
CustomerId+6_     5G      $ Email��6%   $
 1.3* 64    6A   �� *Email+6]     5H      $ Fullname��6%   $ * 67    6A   �� *
FullName+6`     5I      $ Password��6%   $
 2.2* 67    6D   �� *
Password+6c     5J      $ Birthday��6%   $
 1.2* 67    6D   �� *
Birthday+ �� StringFormat$ {0:dd/MM/yyyy}��6�     5K      $ Gender��6%   $
 0.7* 65    6B   �� *Gender+6_     5L      $ CreatedDate��6%   $ * 6:    6D   �� *CreatedDate+$ {0:dd/MM/yyyy}��6�   5N      5O      5P      