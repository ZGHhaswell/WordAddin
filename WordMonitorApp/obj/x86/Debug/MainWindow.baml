   M S B A M L   `   `   `  ����I  EWordMonitorApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:WordMonitorApp.DependPropertiesWordMonitorApp.DependProperties  5   	       WordMonitorApp.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   <xc-clr-namespace:WordMonitorApp.DependProperties     5   	     � Title$  Word文档监视��5   	   $	��600��6   $	��800��6'   8   2WordMonitorApp.DependProperties.DialogResultHelper   DialogResult 5   	   �� *DialogReuslt+.��5      �  � ColumnDefinitions 5   
   �� 5	      $��1* �6   6,   �� 5
      $��3* �6   6,   �� 5      $��1* �6   6,   .+�5   
   �� $��0��6   $��5q�6}   ��6"   �� *	Clients+ �� Mode$ TwoWay=� �� SelectedItem 6O   �� *Client+$ TwoWay=���5      �� 5      .R�5      �� ��6    �� *
ClientInfo+6>   5      5      �� 5   
   $��1��6   $��5q�6z   ��6"   �� *Graphs+$ TwoWay=� 6N   �� *Graph+$ TwoWay=���5      �� 5      .R�5      �� ��6    �� *	GraphInfo+6=   5      5      �� 5   
   -   5        $��button1��5   �   $��选择段落��6   $��2��6B   $��23��6R   $
��Left=�6^   $��	32,12,0,0q�6y   $	��Top=�6�   $��75��6�   ��6    �� *SelectCommand+6�   �� 5   
   -   5        $��button2��5   |   $��删除段落��6   $��23��6B   $
��Left=�6N   $��	32,57,0,0q�6i   $	��Top=�6�   $��75��6�   $��2��6�   ��6    �� *DeleteCommand+6�   �� 5   
   -   5        $��textBox1��5   �   $��2��6>   $��23��6N   $
��Left=�6Z   $��	23,98,0,0q�6u   $	��Top=�6�   $	��120��6�   ��6   �� *InsertContent+$ TwoWay=�6�   �� 5   
   -   5        $��button3��5   }   $��插入段落��63   $��23��6B   $
��Left=�6N   $��
32,140,0,0q�6i   $	��Top=�6�   $��75��6�   $��2��6�   ��6   �� *InsertCommand+6�   �� 5   
   -   5        $��textBox2��5   y   $��23��6>   $
��Left=�6J   $��
23,180,0,0q�6e   $	��Top=�6�   $	��120��6�   $��2��6�   ��6   �� *ModifyContent+$ TwoWay=�6�   �� 5    
   -   5        $��button4��5    }   $��修改段落��63   $��23��6B   $
��Left=�6N   $��
32,220,0,0q�6i   $	��Top=�6�   $��75��6�   $��2��6�   ��6   �� *ModifyCommand+6�   5!      5"      