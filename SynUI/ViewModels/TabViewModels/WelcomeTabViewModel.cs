using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUI.ViewModels.TabViewModels
{
    public class WelcomeTabViewModel : ViewModelBase
    {
        public EditorViewModel? EditorViewModel { get; }

        public WelcomeTabViewModel(EditorViewModel editorViewModel)
        {
            EditorViewModel = editorViewModel;
        }
    }
}
