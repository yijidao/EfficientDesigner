using EfficientDesigner.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace EfficientDesigner.ViewModels
{
    public class DesignViewModel : BindableBase
    {
        public DesignViewModel()
        {
        }


        public ICommand LoadedCommand => new DelegateCommand(() =>
        {
            var list = new List<ControlCategory>();
            list.Add(ControlCategory.GetWPFCategories());
            ControlCategories = new ObservableCollection<ControlCategory>(list);
        });


        private ObservableCollection<ControlCategory> _ControlCategories;
        public ObservableCollection<ControlCategory> ControlCategories
        {
            get { return _ControlCategories; }
            set { SetProperty(ref _ControlCategories, value); }
        }

    }

}
