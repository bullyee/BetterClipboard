   M S B A M L   `   `   `  ����E  AWPFSandbox, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WPFSandbox
WPFSandbox  5   	       WPFSandbox.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Hardcodet.NotifyIcon.Wpf, Version=1.1.0.0, Culture=neutral, PublicKeyToken=682384a853a08aad)tb http://www.hardcodet.net/taskbar  5   	   $localclr-namespace:WPFSandbox  5   	   -   5          � Title$  
MainWindow��5	   	   $	��600��6   $	��300��6)    � Topmost . 
������� � WindowStyle$
 None=�5
   |    � ShowInTaskbar .  .��5      � .+�5   
   3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -   5          Name$ taskbar��5         
IconSource$ /7978227a.ico��5         ToolTipText$ Better Clipboard��5       � ContextMenu 5      �� 5      .@�5      m� -   5         m�Name$ taskbar_toggle��5   -   $
��hide��6   6-   m� 5      -   5        $ taskbar_close��5   .   $��close��6   6.   5      5      �� 5   
   $
��Auto=�6   $
��Auto=�6:   $��Stretch=�5      $��Stretch=�5      $��
10,75,10,0q�5      .��5      �� -   5        	 ��Name$	 clipboard_stack_panel��5      $��Stretch=�5      $��Stretch=�5      |�. 
 �� CanHorizontallyScroll
 . ��5       �� 5!       �� Color# � ��6&   5#      5$      5%      5&      