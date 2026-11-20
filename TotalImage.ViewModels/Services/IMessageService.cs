using System.Threading.Tasks;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Simple questions and messages. There is one method per kind of question; the wording and
/// the buttons belong to each UI.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Warns that formatting erases all data in the current partition and asks whether to continue.
    /// </summary>
    Task<bool> ConfirmFormatAsync();
}
