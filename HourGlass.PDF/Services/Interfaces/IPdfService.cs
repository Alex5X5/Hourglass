namespace Hourglass.PDF.Services.Interfaces;

using System;

public interface IPdfService {

    public void Export(IProgressReporter reporter, DateTime selectedWeek);

    public void Import();
}