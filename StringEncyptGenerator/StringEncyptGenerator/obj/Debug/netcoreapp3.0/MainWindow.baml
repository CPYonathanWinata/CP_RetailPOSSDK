   M S B A M L   `   `   `  ����P  LStringEncyptGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:StringEncyptGeneratorStringEncyptGenerator  5   	   &     StringEncyptGenerator.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:StringEncyptGenerator     5   	     � Title$  
MainWindow��5   	   $	��680��6   $	��375��6)    � 
ResizeMode$ NoResize=�65    � WindowStartupLocation$ CenterScreen=�6K   .��5	      � .+�5
   
   �� -   5         ��Name$ 	txtString��5
      $
��Left=�6%   $��	48,76,0,0q�6@    �� TextWrapping$
 Wrap=�6S   $	��Top=�6g   $	��264��6   
���   �6�   �� 5   
   $��String :��6   $
��Left=�6#   $��	42,53,0,0q�6>   $	��Top=�6Q   �� 5   
   -   5        $ 	txtResult��5      $
��Left=�6%   $��
48,129,0,0q�6@   $
 Wrap=�6T   $	��Top=�6h   $	��264��6�   
���   �$��73��6�    �� 
IsReadOnly$
 True��6�   �� 5   
   $��Result :��6   $
��Left=�6#   $��
42,106,0,0q�6>   $	��Top=�6R   �� 5   
   -   5        $��Encrypt��5      $��Center=�6#   $	��Top=�6@   $��42��6X   $	��108��6d   $��	0,225,0,0q�6�   �� 5   
   $��String Encrypt Generator��6   $��Center=�63   $��0,10,0,0q�6P   $	��Top=�6b   $��20(�6z   $
��Bold"�6�   �� 5   
   -   5        $ 
txtDecrypt��5      $
��Left=�6&   $��
48,355,0,0q�6A   $
 Wrap=�6U   $	��Top=�6i   $	��264��6�   $��73��6�   �� 5   
   -   5        $ 
txtDResult��5      $
��Left=�6&   $��
48,450,0,0q�6A   $
 Wrap=�6U   $	��Top=�6i   $	��264��6�   �� 5   
   -   5        $��Decrypt��5      $��Center=�6#   $��	0,482,0,0q�6@   $	��Top=�6S   $��34��6k   $	��108��6w   6�   �� 5   
   $��String :��6   $
��Left=�6#   $��
42,333,0,0q�6>   $	��Top=�6R   �� 5   
   $��Result :��6   $
��Left=�6#   $��
42,428,0,0q�6>   $	��Top=�6R   �� 5   
   $��String Decrypt Generator��6   $��Center=�63   $��	0,290,0,0q�6P   $	��Top=�6c   $��20(�6{   $
��Bold"�6�   �� 5   
   -   5        $ lblError��5      $��Center=�6$   $��	0,532,0,0q�6A   $
 Wrap=�6T   $	��Top=�6h   $	��264��6�   $��84��6�   $��0,0,0,0q�6�   
���  ��6�   5      5      