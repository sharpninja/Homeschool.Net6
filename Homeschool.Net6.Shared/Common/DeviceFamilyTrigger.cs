namespace Homeschool.App.Common;

using Windows.System.Profile;

// https://docs.microsoft.com/windows/uwp/input-and-devices/designing-for-tv#custom-visual-state-trigger-for-xbox
class DeviceFamilyTrigger : StateTriggerBase
{
    private string _actualDeviceFamily;
    private string _triggerDeviceFamily;

    public string DeviceFamily
    {
        get => _triggerDeviceFamily;
        set
        {
            _triggerDeviceFamily = value;
            _actualDeviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            SetActive(_triggerDeviceFamily == _actualDeviceFamily);
        }
    }
}