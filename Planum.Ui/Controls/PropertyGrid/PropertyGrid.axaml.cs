using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Planum.Ui.Controls
{
    public partial class PropertyGrid : UserControl
    {
        public static readonly StyledProperty<List<ConfigurablePropertyMetadata>> PropertyMetadataProperty =
            AvaloniaProperty.Register<PropertyGrid, List<ConfigurablePropertyMetadata>>(
                nameof(PropertyMetadata), new List<ConfigurablePropertyMetadata>(), 
                notifying: OnSelectedObjectChanged);

        public static readonly StyledProperty<PropertyGridEditControlFactory?> EditControlFactoryProperty =
            AvaloniaProperty.Register<PropertyGrid, PropertyGridEditControlFactory?>(
                nameof(EditControlFactory),
                new PropertyGridEditControlFactory());

        private Grid _gridMain;
        private Control? _firstValueRowEditor;

        public List<ConfigurablePropertyMetadata> PropertyMetadata
        {
            get => GetValue(PropertyMetadataProperty); 
            set => SetValue(PropertyMetadataProperty, value);
        }

        public PropertyGridEditControlFactory? EditControlFactory
        {
            get => GetValue(EditControlFactoryProperty);
            set => SetValue(EditControlFactoryProperty, value);
        }

        public PropertyGrid()
        {
            AvaloniaXamlLoader.Load(this);

            _gridMain = this.FindControl<Grid>("GridMain");
        }

        private static void OnSelectedObjectChanged(IAvaloniaObject sender, bool beforeChanging)
        {
            if (beforeChanging)
                return;
            if (!(sender is PropertyGrid propGrid))
                return;
            
            propGrid.UpdatePropertiesView();
        }
        
        private void UpdatePropertiesView()
        {
            _gridMain.Children.Clear();
            _gridMain.RowDefinitions.Clear();

            var propertyMetadata = PropertyMetadata;
            var lstProperties = new List<ConfigurablePropertyMetadata>(propertyMetadata);
            lstProperties.Sort((left, right) => string.Compare(left.CategoryName, right.CategoryName, StringComparison.Ordinal));

            // Create all controls
            var actRowIndex = 0;
            var actCategory = string.Empty;
            var editControlFactory = EditControlFactory;
            if (editControlFactory == null){ editControlFactory = new PropertyGridEditControlFactory(); }

            foreach (var actProperty in propertyMetadata)
            {
                try
                {
                    // Create category rows
                    if (actProperty.CategoryName != actCategory)
                    {
                        _gridMain.RowDefinitions.Add(new RowDefinition {Height = new GridLength(35)});

                        actCategory = actProperty.CategoryName;

                        var txtHeader = new TextBlock
                        {
                            Text = actCategory
                        };

                        txtHeader.SetValue(Grid.RowProperty, actRowIndex);
                        txtHeader.SetValue(Grid.ColumnSpanProperty, 2);
                        txtHeader.SetValue(Grid.ColumnProperty, 0);
                        txtHeader.Margin = new Thickness(5d, 5d, 5d, 5d);
                        txtHeader.VerticalAlignment = VerticalAlignment.Bottom;
                        txtHeader.FontWeight = FontWeight.Bold;
                        _gridMain.Children.Add(txtHeader);

                        var rect = new Rectangle
                        {
                            Height = 1d,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Margin = new Thickness(5d, 5d, 5d, 0d)
                        };
                        rect.Classes.Add("PropertyGridCategoryHeaderLine");

                        rect.SetValue(Grid.RowProperty, actRowIndex);
                        rect.SetValue(Grid.ColumnSpanProperty, 2);
                        rect.SetValue(Grid.ColumnProperty, 0);
                        _gridMain.Children.Add(rect);

                        actRowIndex++;
                    }

                    // Create row header
                    var ctrlTextContainer = new Border();
                    var ctrlText = new TextBlock();
                    ctrlText.Text = actProperty.PropertyDisplayName;
                    ctrlText.VerticalAlignment = VerticalAlignment.Center;
                    SetToolTip(ctrlText, actProperty.Description);
                    ctrlTextContainer.Height = 35.0;
                    ctrlTextContainer.Child = ctrlText;
                    ctrlTextContainer.SetValue(Grid.RowProperty, actRowIndex);
                    ctrlTextContainer.SetValue(Grid.ColumnProperty, 0);
                    ctrlTextContainer.VerticalAlignment = VerticalAlignment.Top;
                    _gridMain.Children.Add(ctrlTextContainer);

                    // Create and configure row editor
                    var ctrlValueEdit = editControlFactory.CreateControl(actProperty, propertyMetadata);
                    if (ctrlValueEdit != null)
                    {
                        _gridMain.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                        ctrlValueEdit.Margin = new Thickness(0d, 0d, 5d, 0d);
                        ctrlValueEdit.VerticalAlignment = VerticalAlignment.Center;
                        ctrlValueEdit.SetValue(Grid.RowProperty, actRowIndex);
                        ctrlValueEdit.SetValue(Grid.ColumnProperty, 1);
                        ctrlValueEdit.DataContext = actProperty;
                        SetToolTip(ctrlValueEdit, actProperty.Description);
                        _gridMain.Children.Add(ctrlValueEdit);

                        _firstValueRowEditor ??= ctrlValueEdit;
                    }
                    else
                    {
                        _gridMain.RowDefinitions.Add(new RowDefinition(1.0, GridUnitType.Pixel));
                    }

                    actRowIndex++;
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }

        public static void SetToolTip(IAvaloniaObject targetObject, string toolTip)
        {
            if (string.IsNullOrEmpty(toolTip)) { return; }
            targetObject.SetValue(ToolTip.TipProperty, toolTip);
        }
    }
}