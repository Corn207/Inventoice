using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MAUIApp.ViewModels.Messenger;

public class ProductListRefreshMessage() : ValueChangedMessage<object?>(null)
{
}

public class InvoiceListRefreshMessage() : ValueChangedMessage<object?>(null)
{
}

public class ClientListRefreshMessage() : ValueChangedMessage<object?>(null)
{
}

public class ImportReportListRefreshMessage() : ValueChangedMessage<object?>(null)
{
}

public class ExportReportListRefreshMessage() : ValueChangedMessage<object?>(null)
{
}

public class AuditReportListRefreshMessage() : ValueChangedMessage<object?>(null)
{
}

public class UserListRefreshMessage() : ValueChangedMessage<object?>(null)
{
}
