using LearnXR.Core.Utilities;
using Meta.XR.MRUtilityKit;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TrackablesManager : MonoBehaviour
{
    [SerializeField] private GameObject trackedObjectPrefab;
    [SerializeField] private GameObject trackedInfoPrefab;
    [SerializeField] private GameObject trackedBoundsPrefab;
    
    public void OnTrackableAdded(MRUKTrackable trackable)
    {
        SpatialLogger.Instance.LogInfo($"Trackable of type {trackable.TrackableType} added");
        var trackedObjectInstance = Instantiate(trackedObjectPrefab, trackable.transform);
        
        var trackedBoundsInstance = Instantiate(trackedBoundsPrefab, trackedObjectInstance.transform);
        var boundingAreaRect = trackable.PlaneRect.Value;
        
        if (trackable.TrackableType == OVRAnchor.TrackableType.QRCode 
            && trackable.MarkerPayloadString != null)
        {
            var trackedObjectInfo = Instantiate(trackedInfoPrefab, trackedObjectInstance.transform);
            trackedObjectInfo.GetComponentInChildren<TextMeshProUGUI>()
                    .text = $"<color=red>QR Code Payload:</color> {trackable.MarkerPayloadString}";
            
            trackedBoundsInstance.transform.localScale = new Vector3(boundingAreaRect.width, boundingAreaRect.height, 0.01f);
            trackedBoundsInstance.transform.localPosition = new Vector3(boundingAreaRect.center.x, boundingAreaRect.center.y, 0f);
        }
        else if(trackable.TrackableType == OVRAnchor.TrackableType.Keyboard)
        {
            trackedBoundsInstance.transform.localScale = trackable.VolumeBounds.Value.size;
            trackedBoundsInstance.transform.localPosition = Vector3.zero;
        }
    }

    public void OnTrackableRemoved(MRUKTrackable trackable)
    {
        SpatialLogger.Instance.LogInfo($"Trackable removed: {trackable.name}");
        Destroy(trackable.gameObject);
    }
}