using KeyShare.Core.Contracts.Services.Application;
using KeyShare.Core.Contracts.Services.Db;
using KeyShare.Core.Models.Presentation;

namespace KeyShare.Core.Services.Application;
public class RetrieveAllPiecesCommand: IRetrieveAllPiecesCommand
{
    private readonly ISecretPieceRepository _secretPieceRepository;
    public RetrieveAllPiecesCommand(ISecretPieceRepository secretPieceRepository)
    {
        _secretPieceRepository = secretPieceRepository;
    }
    public IEnumerable<PresentationSecretPiece> Execute()
    {
        var pieces = new List<PresentationSecretPiece>();
        var dbPieces = _secretPieceRepository.GetAllSecretPieces();
        foreach (var dbPiece in dbPieces)
        {
            pieces.Add(
                new PresentationSecretPiece()
                {
                    ID = dbPiece.ID,
                    KeyID = dbPiece.KeyID,
                    X = dbPiece.X,
                    Y = dbPiece.Y,
                }
            );
        }
        return pieces;
    }

}
