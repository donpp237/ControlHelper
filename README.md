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
        </Grid.RowDefinitions>
        
        <control:PlaceHolderTextBox Grid.Row="0"
                                    VerticalAlignment="Center"
                                    HorizontalAlignment="Center"
                                    Width="100"
                                    PlaceHolderText="Input Contents..." 
                                    Text="{Binding Name}"/>

        <control:ColorPicker Grid.Row="1"
                             Width="100"
                             HorizontalAlignment="Center"/>
    </Grid>
</Window>
```

### Result