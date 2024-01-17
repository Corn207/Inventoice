using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApp.ViewModels.Users;
public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		throw new NotImplementedException();
	}
}
