namespace Hourglass.PDF.Services.Interfaces;

using System;

public interface IPdfService {

    public void Export(IProgressReporter reporter);

    public void Import();
}