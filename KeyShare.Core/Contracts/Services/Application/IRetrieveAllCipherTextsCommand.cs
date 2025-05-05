using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShare.Core.Models.Presentation;

namespace KeyShare.Core.Contracts.Services.Application;
public interface IRetrieveAllCipherTextsCommand
{
    IEnumerable<PresentationCipherText> Execute();
}
