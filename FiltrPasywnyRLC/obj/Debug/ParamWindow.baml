   M S B A M L   `   `   `  ????J  FFiltrPasywnyRLC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FiltrPasywnyRLCFiltrPasywnyRLC  5   	   !    FiltrPasywnyRLC.ParamWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FiltrPasywnyRLC     5   	     ? Title$  Parameters Window??5   	   $	??210??6#   $	??400??60   $	??400??5	   	   $	??200??5
   	    ? SizeToContent$ WidthAndHeight=?5   	    ? 
ResizeMode$ NoResize=?5   	    ? ShowInTaskbar .   ? WindowStartupLocation$ CenterOwner=?5   	   .??5      ? -   5        $??
parameters??5       ? ColumnDefinitions 5   
   ?? 5      $??1* ?6   6,   ?? 5      $??1* ?6   6,    ? RowDefinitions 5   
   ?? 5      $
??auto ?6   6,   ?? 5      $
??auto ?6   6,   ?? 5      $
??auto ?6   6,   .+?5   
   ?? $??Supply parameters??6   $??0??6.   $??0??6;   .??5      ??  ?? Orientation$ Vertical=?6   .+?5      ?? $ 
Horizontal=?6   $??5,5,0,5q?66   .+?5      ?? $??Voltage Max [V]:??6    $	??110??68   6F   ?? 5      -   5        $??txtMagnitude??5      $??60??62   6?   5      ?? 5       $ 
Horizontal=?6   $??5,0,0,5q?66   .+?5!      ?? $??Freq. min [Hz]:??6    $	??110??67   6E   ?? 5"      -   5        $??
txtFreqMin??5"      $??60??60   6=   5#      ?? 5$      $ 
Horizontal=?6   $??5,0,0,5q?66   .+?5%      ?? $??Freq. max [Hz]:??6    $	??110??67   6E   ?? 5&      -   5        $??
txtFreqMax??5&      $??60??60   6=   5'      5(      5)      ?? 5+   
   $??Filter parameters??6   $??0??6.   $??1??6;   $??2??6K   .??5,      ?? $ Vertical=?6   .+?5-      ?? $ 
Horizontal=?6   $??0,5,0,5q?66   .+?5.      ?? $??Resistance R1 [Ohm]:??6    $	??115??6<   6K   ?? 5/      -   5        $??txtResistance??5/      $??60??63   6@   50      ?? 51      $ 
Horizontal=?6   $??0,5,0,5q?66   .+?52      ?? $??Resistance R2 [Ohm]:??6    $	??115??6<   6K   ?? 53      -   5        $??txtResistance2??53      $??60??64   6A   54      ?? 55      $ 
Horizontal=?6   $??0,0,0,5q?66   .+?56      ?? $??Inductance [H]:??6    $	??115??67   6E   ?? 57      -   5        $??txtInductance??57      $??60??63   6@   58      ?? 59      $ 
Horizontal=?6   $??0,0,0,0q?66   .+?5:      ?? $??Capacitance [F]:??6    $	??115??68   6F   ?? 5;      -   5        $??txtCapacitance??5;      $??60??64   6A   5<      5=      5>      ?? 5@   
   $ 
Horizontal=?6   $??1??6.   $??0??6;   $??2??6K   $??0,5,0,10q?6_   .+?5A      ?? $??Number of Points:??6   $	??110??61   $??10,0,0,0q?6=   6Q   ?? 5B      -	   5        $??	txtPoints??5B      $??60??6'   64   5C      ? 5E   
   $??2??6   $??0??6   $??2??6,   $??
10,10,10,0q?6@   .+?5F      ?? -
   5        $??btnOK??5F      $??OK??6"   $	??100??6/    ?? 	IsDefault . $
??Left=?6`   6}   ?? 5G      -   5        $??btnSave??5G      $??Save to png??6$   $	??100??6:   	 ?? IsCancel	 . $??Center=?6V   6?   ?? 5H      -   5        $??	btnCancel??5H      $??Cancel??6&   $	??100??67   	 . $??Right=?6k   6?   5I      5L      5M      