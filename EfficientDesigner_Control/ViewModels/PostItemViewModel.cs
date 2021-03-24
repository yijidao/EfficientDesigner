using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels
{
    public class PostItemViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private DateTime _updateTime;
        public DateTime UpdateTime
        {
            get => _updateTime;
            set => SetProperty(ref _updateTime, value);
        }

        private bool _editing;
        public bool Editing
        {
            get => _editing;
            set => SetProperty(ref _editing, value);
        }

        public PostItemViewModel(string name)
        {
            Name = name;
            UpdateTime = DateTime.Now;

            ChangeEditCommand = new DelegateCommand(() => { Editing = !Editing; });

            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Editing))
            {
                if (!Editing) UpdateTime = DateTime.Now;
            }
        }

        public ICommand ChangeEditCommand { get; }
    }
}
