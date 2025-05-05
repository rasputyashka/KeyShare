using KeyShare.Core.Models.Presentation;

namespace KeyShare.Core.Contracts.Services.Application;
public interface IRetrieveAllPiecesCommand
{
    IEnumerable<PresentationSecretPiece> Execute();
}
