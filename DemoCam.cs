using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class DemoCam : MonoBehaviour
{
    public static DemoCam Instance;
    private CinemachineVirtualCamera _vcam;

    public async void Show(Transform target)
    {
        _vcam.Follow = target;
        _vcam.LookAt = target;
        gameObject.SetActive(true);

        await Task.Delay(2000);
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
        _vcam = GetComponent<CinemachineVirtualCamera>();
        gameObject.SetActive(false);
    }
}
