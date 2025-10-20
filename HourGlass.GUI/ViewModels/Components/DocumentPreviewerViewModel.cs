namespace Hourglass.GUI.ViewModels.Components;

using Hourglass.GUI.Views.Components;

using System.Collections.ObjectModel;

public class DocumentPreviewerViewModel : ViewModelBase {

	private readonly ComponentModelFactory<DocumentPreviewerView> componentFactory;

	public DocumentPreviewerViewModel() : this(null) {
	}

	public DocumentPreviewerViewModel(ComponentModelFactory<DocumentPreviewerView> componentFactory):base() {
		this.componentFactory = componentFactory;
	}
}
