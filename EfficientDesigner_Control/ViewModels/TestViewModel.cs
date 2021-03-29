using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EfficientDesigner_Client_Common.Services;
using EfficientDesigner_Control.Common;
using Prism.Commands;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels
{
    [HasService(nameof(ILayoutService),nameof(ILayoutService.GetTestList))]
    public class TestViewModel : VisualViewModelBase
    {
        private readonly ILayoutService _layoutService;
        private ObservableCollection<string> _datas = new ObservableCollection<string>();
        public ObservableCollection<string> Datas
        {
            get => _datas;
            set => SetProperty(ref _datas, value);
        }

        public TestViewModel(ILayoutService layoutService)
        {
            _layoutService = layoutService;
            LoadDataCommand = new DelegateCommand(async () => await LoadData());
        }

        private async Task LoadData()
        {
            var result = await _layoutService.GetTestList(LayoutId, ViewId);
            Datas = new ObservableCollection<string>(result);
        }

        public ICommand LoadDataCommand { get; }
    }
}
