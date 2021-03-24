using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EfficientDesigner_Service;
using EfficientDesigner_Service.ServiceImplements;
using Prism.Commands;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels
{
    public class LayoutListViewModel : BindableBase
    {
        private ObservableCollection<LayoutItemViewModel> _layoutItems = new ObservableCollection<LayoutItemViewModel>();
        public ObservableCollection<LayoutItemViewModel> LayoutItems
        {
            get => _layoutItems;
            set => SetProperty(ref _layoutItems, value);
        }

        public LayoutListViewModel()
        {
            LoadDataCommand = new DelegateCommand(async () => await LoadData());
        }

        private async Task LoadData()
        {
            var datas = await ServiceFactory.GetLayoutService().GetLayouts();
            var vms = datas.Select(x => new LayoutItemViewModel(x));
            LayoutItems.AddRange(vms);
        }

        public ICommand LoadDataCommand { get; }

    }
}
