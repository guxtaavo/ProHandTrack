using UnityEngine;
using Meta.XR.MRUtilityKit;

#if META_QR_SUPPORTED
using Meta.XR.QR;
#endif

public class MultiPrefabTrackablesManager : MonoBehaviour
{
    [Header("Prefabs a serem usados conforme o QR Code detectado")]
    public GameObject axisPrefab;
    public GameObject arrowPrefab;

    public void OnTrackableAdded(MRUKTrackable trackable)
    {
#if META_QR_SUPPORTED
        QRCode qrCode = trackable.GetComponent<QRCode>();
        if (qrCode == null)
        {
            Debug.LogWarning($"Trackable sem QR: {trackable.name}");
            return;
        }
        string qrContent = qrCode.Text;
#else
        Debug.LogWarning("QR Code Tracking não está disponível neste build!");
        string qrContent = trackable.name;
#endif

        GameObject prefabToSpawn = null;
        if (qrContent.ToLower().Contains("axis"))
            prefabToSpawn = axisPrefab;
        else if (qrContent.ToLower().Contains("arrow"))
            prefabToSpawn = arrowPrefab;

        if (prefabToSpawn != null)
        {
            GameObject spawned = Instantiate(prefabToSpawn, trackable.transform.position, trackable.transform.rotation);
            spawned.transform.SetParent(trackable.transform, true);
            Debug.Log($"Prefab '{prefabToSpawn.name}' instanciado para o QR '{qrContent}'");
        }
    }

    public void OnTrackableRemoved(MRUKTrackable trackable)
    {
        foreach (Transform child in trackable.transform)
            Destroy(child.gameObject);
    }
}
