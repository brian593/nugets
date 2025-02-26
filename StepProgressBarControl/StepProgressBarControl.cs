using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace StepProgressBarControl
{
    public class StepProgressBarControl : StackLayout
    {
        Button _lastStepSelected;

        public static readonly BindableProperty StepsProperty = BindableProperty.Create(nameof(Steps), typeof(int), typeof(StepProgressBarControl), 0);
        public static readonly BindableProperty StepSelectedProperty = BindableProperty.Create(nameof(StepSelected), typeof(int), typeof(StepProgressBarControl), 0, defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty StepColorProperty = BindableProperty.Create(nameof(StepColor), typeof(Color), typeof(StepProgressBarControl), Colors.Black, defaultBindingMode: BindingMode.TwoWay);

        public Color StepColor
        {
            get => (Color)GetValue(StepColorProperty);
            set => SetValue(StepColorProperty, value);
        }

        public int Steps
        {
            get => (int)GetValue(StepsProperty);
            set => SetValue(StepsProperty, value);
        }

        public int StepSelected
        {
            get => (int)GetValue(StepSelectedProperty);
            set => SetValue(StepSelectedProperty, value);
        }

        public StepProgressBarControl()
        {
            Orientation = StackOrientation.Horizontal;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Padding = new Thickness(10, 0);
            Spacing = 0;
            AddStyles();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == StepsProperty.PropertyName)
            {
                Children.Clear(); // Limpiar hijos previos
                for (int i = 0; i < Steps; i++)
                {
                    var button = new Button
                    {
                        Text = $"{i + 1}",
                        AutomationId = $"{i + 1}",
                        Style = Resources["unSelectedStyle"] as Style
                    };

                    button.Clicked += Handle_Clicked;
                    Children.Add(button);

                    if (i < Steps - 1)
                    {
                        var separatorLine = new BoxView
                        {
                            BackgroundColor = Colors.Silver,
                            HeightRequest = 1,
                            WidthRequest = 5,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        Children.Add(separatorLine);
                    }
                }
            }
            else if (propertyName == StepSelectedProperty.PropertyName)
            {
                var children = Children.FirstOrDefault(p => p is Button btn && !string.IsNullOrEmpty(btn.AutomationId) && Convert.ToInt32(btn.AutomationId) == StepSelected);
                if (children != null) SelectElement(children as Button);
            }
            else if (propertyName == StepColorProperty.PropertyName)
            {
                AddStyles();
            }
        }

        void Handle_Clicked(object sender, EventArgs e)
        {
            SelectElement(sender as Button);
        }

        void SelectElement(Button elementSelected)
        {
            if (_lastStepSelected != null) _lastStepSelected.Style = Resources["unSelectedStyle"] as Style;
            elementSelected.Style = Resources["selectedStyle"] as Style;
            StepSelected = Convert.ToInt32(elementSelected.Text);
            _lastStepSelected = elementSelected;
        }

        void AddStyles()
        {
            var unselectedStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = BackgroundColorProperty, Value = Colors.Transparent },
                    new Setter { Property = Button.BorderColorProperty, Value = StepColor },
                    new Setter { Property = Button.TextColorProperty, Value = StepColor },
                    new Setter { Property = Button.BorderWidthProperty, Value = 0.5 },
                    new Setter { Property = Button.CornerRadiusProperty, Value = 20 }, // Cambio a Button.CornerRadiusProperty
                    new Setter { Property = HeightRequestProperty, Value = 40 },
                    new Setter { Property = WidthRequestProperty, Value = 40 }
                }
            };

            var selectedStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = BackgroundColorProperty, Value = StepColor },
                    new Setter { Property = Button.TextColorProperty, Value = Colors.White },
                    new Setter { Property = Button.BorderColorProperty, Value = StepColor },
                    new Setter { Property = Button.BorderWidthProperty, Value = 0.5 },
                    new Setter { Property = Button.CornerRadiusProperty, Value = 20 }, // Cambio a Button.CornerRadiusProperty
                    new Setter { Property = HeightRequestProperty, Value = 40 },
                    new Setter { Property = WidthRequestProperty, Value = 40 },
                    new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold }
                }
            };

            Resources = new ResourceDictionary
            {
                { "unSelectedStyle", unselectedStyle },
                { "selectedStyle", selectedStyle }
            };
        }
    }
}