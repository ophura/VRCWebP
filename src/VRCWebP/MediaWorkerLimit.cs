using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VRCWebP;

/// <summary>Bounds committed worker memory and terminates owned media workers if the application exits.</summary>
internal sealed partial class MediaWorkerLimit : IDisposable
{
    internal const long MaximumBytes = 1073741824;

    private const uint JobObjectLimitProcessMemory = 0x100;

    private const uint JobObjectLimitJobMemory = 0x200;

    private const uint JobObjectLimitKillOnJobClose = 0x2000;

    private const int JobObjectExtendedLimitInformation = 9;

    private readonly SafeFileHandle Job = CreateJobObjectW(0, 0);

    private readonly long AllowedBytes = MaximumBytes;

    internal MediaWorkerLimit(long Bytes = MaximumBytes)
    {
        if (Bytes is < 33554432 or > MaximumBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(Bytes));
        }

        AllowedBytes = Bytes;
    }

    /// <summary>Applies per-process and aggregate job commitment ceilings in bytes. Attachment failure requires the caller to kill the unbounded child before any further I/O.</summary>
    internal unsafe void Attach(Process Worker)
    {
        ExtendedLimits Limits = new()
        {
            Basic = new BasicLimits
            {
                Flags = JobObjectLimitProcessMemory | JobObjectLimitJobMemory | JobObjectLimitKillOnJobClose
            },

            ProcessMemory = (nuint)AllowedBytes,

            JobMemory = (nuint)AllowedBytes
        };

        if (Job.IsInvalid || !SetInformationJobObject(Job, JobObjectExtendedLimitInformation, &Limits, (uint)sizeof(ExtendedLimits))
            || !AssignProcessToJobObject(Job, Worker.SafeHandle))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "Unable to apply the media worker's memory safety limit.");
        }
    }

    internal unsafe long PeakBytes
    {
        get
        {
            ExtendedLimits Limits = default;

            return QueryInformationJobObject(Job, JobObjectExtendedLimitInformation, &Limits, (uint)sizeof(ExtendedLimits), 0)
                ? (long)Limits.PeakJobMemory : 0;
        }
    }


    public void Dispose()
    {
        Job.Dispose();
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BasicLimits
    {
        internal long ProcessTime;

        internal long JobTime;

        internal uint Flags;

        internal nuint MinimumWorkingSet;

        internal nuint MaximumWorkingSet;

        internal uint ActiveProcesses;

        internal nuint Affinity;

        internal uint Priority;

        internal uint Scheduling;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ExtendedLimits
    {
        internal BasicLimits Basic;

        internal ulong ReadOperations;

        internal ulong WriteOperations;

        internal ulong OtherOperations;

        internal ulong ReadBytes;

        internal ulong WriteBytes;

        internal ulong OtherBytes;

        internal nuint ProcessMemory;

        internal nuint JobMemory;

        internal nuint PeakProcessMemory;

        internal nuint PeakJobMemory;
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial SafeFileHandle CreateJobObjectW(nint Attributes, nint Name);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static unsafe partial bool SetInformationJobObject(SafeFileHandle Job, int Class, void* Information, uint Length);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AssignProcessToJobObject(SafeFileHandle Job, SafeProcessHandle Process);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static unsafe partial bool QueryInformationJobObject(SafeFileHandle Job, int Class, void* Information, uint Length, nint Returned);
}
