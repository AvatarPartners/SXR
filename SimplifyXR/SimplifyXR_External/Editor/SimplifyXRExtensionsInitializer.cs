using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if USING_ARFOUNDATION && SIMPLIFYXR_ARFOUNDATION_PRESENT
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#endif

namespace SimplifyXR
{
    /// <summary>
    /// Sets and prepares editor functionality for presence of 3rd party extensions
    /// DO NOT MOVE, REMOVE OR MODIFY.
    /// </summary>
    [InitializeOnLoad]
    public static class SimplifyXRExtensionsInitializer
    {
        static SimplifyXRExtensionsInitializer()
        {
            SetVisionLibEditorExtensions();
            SetARFoundationEditorExtensions();
        }

        static void SetVisionLibEditorExtensions()
        {
#if USING_VISIONLIB

            SimplifyXRAccessManager.Instance.SetVisionLibEditorExtensionBehavior(new VisionLibEditorExtensionBehavior());

            SimplifyXRPostprocessor.SimplifyXRVisionLibPresent = true;
            SimplifyXRAccessManager.Instance.SetVisionLibExtensionBehavior(new VisionLibEditorDLLExtensionBehavior());
            CheckVisionLibVersion();
#else
            SimplifyXRPostprocessor.SimplifyXRVisionLibPresent = false;
#endif
        }

        static void SetARFoundationEditorExtensions()
        {
#if USING_ARFOUNDATION && SIMPLIFYXR_ARFOUNDATION_PRESENT
            SimplifyXRAccessManager.Instance.SetARFoundationEditorExtensionBehavior(new ARFoundationEditorExtensionBehavior());
#endif
        }

        static void CheckVisionLibVersion()
        {
#if USING_VISIONLIB
            Version installedVisionLibVersion;
            if (VLUnitySdk.GetVersionString(out string version))
                installedVisionLibVersion = new Version(version);
            else
                installedVisionLibVersion = new Version(SimplifyXRAccessManager.Instance.GetSupportedVisionLibVersion());

            var supportedVisionLibVersion = new Version(SimplifyXRAccessManager.Instance.GetSupportedVisionLibVersion());

            if (installedVisionLibVersion != null && supportedVisionLibVersion != null)
            {
                var result = installedVisionLibVersion.CompareTo(supportedVisionLibVersion);
                if (result > 0)
                    Debug.LogWarningFormat("WARNING: Your installed version ({0}) of VisionLib is ahead of the SimplifyXR supported version ({1}) of VisionLib", installedVisionLibVersion, supportedVisionLibVersion);
                else if (result < 0)
                    Debug.LogWarningFormat("WARNING: Your installed version ({0}) of VisionLib is behind the SimplifyXR supported version ({1}) of VisionLib", installedVisionLibVersion, supportedVisionLibVersion);
            }
#endif
        }
    }

    class VisionLibEditorExtensionBehavior : IManageVisionLibEditorExtensions
    {
        #region IManageVisionLibEditorExtensions implementation
        public void ConfigureVLWorkerBehaviour(GameObject go, string visionLibLicenseFileName, string visionLibCameraCalFileName)
        {
#if USING_VISIONLIB
            if (go != null)
            {
                var workerBehavior = go.GetComponent<VLWorkerBehaviour>();
                if (workerBehavior != null)
                {
                    if (!string.IsNullOrEmpty(visionLibLicenseFileName))
                        workerBehavior.licenseFile.path = visionLibLicenseFileName;
                    if (!string.IsNullOrEmpty(visionLibCameraCalFileName))
                        workerBehavior.calibrationDataBaseURI = "project_dir:/" + visionLibCameraCalFileName;
                    workerBehavior.targetFPS = 30;
                    return;
                }
            }
            Debug.LogError("Check the VLWorkerBehaviour on the VLHoloLensTracker or VLCamera prefab as there were issues configuring it.");
#endif
        }

        public void ConfigureVLHololensTrackerBehaviour(GameObject go, GameObject contentHolder)
        {
#if USING_VISIONLIB
            if (go != null)
            {
                var hololensStabilizationBehavior = go.GetComponent<VLHoloLensStabilizationPlaneBehaviour>();
                if (hololensStabilizationBehavior != null)
                {
                    UnityEngine.Object.DestroyImmediate(hololensStabilizationBehavior);
                }
                var vLDetectScreenChange = go.GetComponent<VLDetectScreenChangeBehaviour>();
                if (vLDetectScreenChange != null)
                {
                    UnityEngine.Object.DestroyImmediate(vLDetectScreenChange);
                }
                var hololensTrackerBehavior = go.GetComponent<VLHoloLensTrackerBehaviour>();
                if (hololensTrackerBehavior != null)
                {
                    hololensTrackerBehavior.smoothTime = 0f;
                    if (contentHolder != null)
                    {
                        hololensTrackerBehavior.content = contentHolder;
                        return;
                    }
                }
            }
            Debug.LogError("Check the VLHoloLensTracker prefab as there were issues configuring it.");
#endif
        }

        public void ConfigureVLHololensInitCamera(GameObject go)
        {
#if USING_VISIONLIB
            if (go != null)
            {
                var hololensInitCameraBehavior = go.GetComponent<VLHoloLensInitCameraBehaviour>();
                if (hololensInitCameraBehavior != null)
                {
                    hololensInitCameraBehavior.keepUpright = true;
                }
                var cam = go.GetComponent<Camera>();
                if (cam != null)
                {
                    cam.nearClipPlane = 0.3f;
                    cam.farClipPlane = 100f;
                    cam.fieldOfView = 16.5f;
                    // cam.clearFlags = CameraClearFlags.Color;
                }
                return;
            }
            Debug.LogError("Check the VLHoloLensInitCamera prefab as there were issues configuring it.");
#endif
        }
        #endregion
    }
    class ARFoundationEditorExtensionBehavior : IManageARFoundationEditorExtensions
    {
        public void AddARFoundationAdapterComponent(GameObject contentHolder, bool TrackAllTargetsInDatabase, string IndividualTargetToTrack, ScriptableObject imageDatabase, GameObject sessionOrigin, bool IsPlanar)
        {
#if USING_ARFOUNDATION && SIMPLIFYXR_ARFOUNDATION_PRESENT
            if (IsPlanar)
            {
                ARFoundationAdapterPlanes adapter = sessionOrigin.AddComponent<ARFoundationAdapterPlanes>();
                adapter.contentHolder = contentHolder;
                adapter.stopMovingAfterFirstHit = true;
            }
            else
            {
                ARFoundationAdapter adapter = sessionOrigin.AddComponent<ARFoundationAdapter>();

                adapter.contentHolder = contentHolder;
                adapter.TrackAll = TrackAllTargetsInDatabase;

                bool nameInReferenceLibrary = false;

                if (imageDatabase.GetType().IsAssignableFrom(typeof(XRReferenceImageLibrary)))
                {
                    foreach (XRReferenceImage image in (XRReferenceImageLibrary)imageDatabase)
                    {
                        if (image.name.Equals(IndividualTargetToTrack))
                        {
                            adapter.defaultTexture = image.texture;
                            nameInReferenceLibrary = true;
                        }
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Image Library Missing", "You did not assign an ImageLibrary. Locate teh ARTrackedImageManager to manually assign this object", "Ok");
                }

                if (!nameInReferenceLibrary && !TrackAllTargetsInDatabase)
                    EditorUtility.DisplayDialog("Image not in Library", "The image named " + IndividualTargetToTrack + " is not in the Reference Library. This image will not be tracked until added to the Reference Library", "OK");

                adapter.ImageTargetName = IndividualTargetToTrack;
            }
#endif
        }

        public void CreateARSession()
        {
#if USING_ARFOUNDATION && SIMPLIFYXR_ARFOUNDATION_PRESENT
            EditorApplication.ExecuteMenuItem("GameObject/XR/AR Session");
#endif
        }

        public GameObject CreateARSessionOrigin(int targetType, ScriptableObject imageDatabase)
        {
#if USING_ARFOUNDATION && SIMPLIFYXR_ARFOUNDATION_PRESENT
            EditorApplication.ExecuteMenuItem("GameObject/XR/AR Session Origin");

            GameObject sessionOrigin = GameObject.FindObjectOfType<ARSessionOrigin>().gameObject;

            if (targetType == 0)
            {
                ARTrackedImageManager imageManager = sessionOrigin.AddComponent<ARTrackedImageManager>();

                if (imageDatabase.GetType().IsAssignableFrom(typeof(XRReferenceImageLibrary)))
                {
                    imageManager.referenceLibrary = imageDatabase as XRReferenceImageLibrary;
                    imageManager.maxNumberOfMovingImages = 1;
                }
                else
                {
                    EditorUtility.DisplayDialog("Image Library Missing", "You did not assign an ImageLibrary. Locate teh ARTrackedImageManager to manually assign this object", "Ok");
                }

            }
            else if (targetType == 1)
            {
                ARPlaneManager planeManager = sessionOrigin.AddComponent<ARPlaneManager>();
                GameObject prefab = SimplifyXREditorUtilityMethods.FindPrefabInAssets("SXR Default Plane");
                if (prefab != null)
                    planeManager.planePrefab = prefab;
                else
                    SimplifyXRDebug.SimplifyXRLog(SimplifyXRDebug.Type.Error, "[DEVELOPER ERROR] No SXR Default Plane prefab in project", SimplifyXRDebug.Args());
            }

            sessionOrigin.AddComponent<ARRaycastManager>();

            Camera theCam = sessionOrigin.GetComponentInChildren<Camera>();
            theCam.tag = "MainCamera";

            return sessionOrigin;
#else
            return null;
#endif
        }
    }
}