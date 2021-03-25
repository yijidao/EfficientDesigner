using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EfficientDesigner_Client_Common.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels
{
    public class LayoutListViewModel : BindableBase
    {
        private readonly ILayoutService _layoutService;
        private ObservableCollection<LayoutItemViewModel> _layoutItems = new ObservableCollection<LayoutItemViewModel>();
        public ObservableCollection<LayoutItemViewModel> LayoutItems
        {
            get => _layoutItems;
            set => SetProperty(ref _layoutItems, value);
        }

        public LayoutListViewModel(ILayoutService layoutService)
        {
            _layoutService = layoutService;
            LoadDataCommand = new DelegateCommand(async () => await LoadData());
        }

        private async Task LoadData()
        {
            var datas = await _layoutService.GetLayoutList();
            var vms = datas.Select(x => new LayoutItemViewModel(x));
            LayoutItems.Clear();
            LayoutItems.AddRange(vms);
            //LayoutItems = new ObservableCollection<LayoutItemViewModel>(vms);
        }

        public ICommand LoadDataCommand { get; }

    }
}
