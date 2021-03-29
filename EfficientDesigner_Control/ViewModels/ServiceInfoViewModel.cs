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
    public class ServiceInfoViewModel : BindableBase
    {
        private readonly ILayoutService _layoutService;
        private ObservableCollection<ServiceInfoItemViewModel> _serviceInfoList = new ObservableCollection<ServiceInfoItemViewModel>();
        public ObservableCollection<ServiceInfoItemViewModel> ServiceInfoList
        {
            get => _serviceInfoList;
            set => SetProperty(ref _serviceInfoList, value);
        }

        public ServiceInfoViewModel(ILayoutService layoutService)
        {
            _layoutService = layoutService;
            LoadDataCommand = new DelegateCommand(async () => await LoadData());
        }

        private async Task LoadData()
        {
            var datas = await _layoutService.GetServiceList();
            ServiceInfoList.AddRange(datas.Select(x => new ServiceInfoItemViewModel(x)));
        }

        public ICommand LoadDataCommand { get; }
    }
}
