# ControlHelper

## Purpose

Helper that can assist in using useful controls of View in MVVM patterns

## Example

### View
```xml
<Window x:Class="MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:FrameworkTest"
        xmlns:viewModels="clr-namespace:ViewModels"
        xmlns:control="clr-namespace:ControlHelper;assembly=ControlHelper"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">

    <Window.Resources>
        <viewModels:ViewModel x:Key="viewModel"/>
    </Window.Resources>
    
    <Grid DataContext="{Binding Source={StaticResource viewModel}}">
        <Grid.RowDefinitions>
            <RowDefinition/>
            <RowDefinition/>
            <RowDefinition/>
        </Grid.RowDefinitions>
        
        <GroupBox Header="PlaceHolderTextBox"
                  FontWeight="Bold"
                  Grid.Row="0"
                  Width="200"
                  VerticalAlignment="Center"
                  HorizontalAlignment="Center">
            <control:PlaceHolderTextBox Width="100"
                                        PlaceHolderText="Input Contents..." 
                                        Text="{Binding Name}"/>
        </GroupBox>

        <GroupBox Header="OpenFileDialog"
                  FontWeight="Bold"
                  Grid.Row="1"
                  Width="200"
                  VerticalAlignment="Center"
                  HorizontalAlignment="Center">
            <control:OpenFileDialog Filter="All|*" 
                                    PathFindBtnName="..."
                                    PlaceHolderText="../FileName.*"
                                    Path="{Binding Name}"/>
        </GroupBox>

        <GroupBox Header="ColorPicker"
                  FontWeight="Bold"
                  Grid.Row="2"
                  Width="200"
                  HorizontalAlignment="Center">
            <control:ColorPicker Width="100"/>
        </GroupBox>
    </Grid>
</Window>
```

### Result
![image](https://github.com/donpp237/ControlHelper/assets/137162873/d25f7715-80a4-4fe4-8118-90c5e33b96eb)
