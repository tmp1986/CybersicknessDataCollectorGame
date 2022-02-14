namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	[RequireComponent(typeof (IKDVC_DriveSystem))] public class IKDVC_Audio : MonoBehaviour
	{
        private IKDVC_DriveSystem vehicleController;
        public AudioSource shiftSource, lowAccelerationSource, lowDecelerationSource, highAccelerationSource, highDecelerationSource;
        public float pitchMultiplier = 1f, lowPitchMin = 1f, lowPitchMax = 6f, highPitchMultiplier = 0.25f;	       
        private float volumeLimit, pitch, accFade, decFade, highFade, lowFade;
        public bool isPlayingSkidAudio;

        public void SetVolumeLimit (float _engineVolumeLimit, float _shiftVolumeLimit)
        {
            volumeLimit = _engineVolumeLimit;
            shiftSource.volume = _shiftVolumeLimit;
        }

        void OnEnable ()
        {
            vehicleController = GetComponent<IKDVC_DriveSystem>(); // get the carcontroller ( this will not be null as we have require component)
            vehicleController.OnChangedGear += PlayShiftSound;
            if (vehicleController.wheelSettings.enableSkid)
            {
                Transform skidTrailParent = new GameObject("Skid Trails - Detached").transform;
                for (int i = 0; i < 4; i++)
                {
                    vehicleController.wheels[i].wheel.canPlayAudio = vehicleController.wheelSettings.enableSkidAudio;
                    vehicleController.wheels[i].wheel.skidTrailParent = skidTrailParent;
                }
            }
            lowAccelerationSource.Play();
            lowDecelerationSource.Play();
            highAccelerationSource.Play();
            highDecelerationSource.Play();
            InvokeRepeating ("UpdateAudio", 0.0f, 0.2f);
        }

        void OnDisable ()
        {
            vehicleController.OnChangedGear -= PlayShiftSound;
            lowAccelerationSource.Stop();
            lowDecelerationSource.Stop();
            highAccelerationSource.Stop();
            highDecelerationSource.Stop();
            CancelInvoke ("UpdateAudio");
        }
        
        public void PlayShiftSound ()
        {
            shiftSource.Play ();
        }        

        private void UpdateAudio()
        {
            pitch = IKD_StaticUtility.ULerp(lowPitchMin, lowPitchMax, vehicleController.RevsAudio); // The pitch is interpolated between the min and max values, according to the car's revs.
            pitch = Mathf.Min(lowPitchMax, pitch); // clamp to minimum pitch (note, not clamped to max for high revs while burning out)                
            lowAccelerationSource.pitch = pitch*pitchMultiplier; // adjust the pitches based on the multipliers
            lowDecelerationSource.pitch = pitch*pitchMultiplier;
            highAccelerationSource.pitch = pitch*highPitchMultiplier*pitchMultiplier;
            highDecelerationSource.pitch = pitch*highPitchMultiplier*pitchMultiplier;                
            accFade = Mathf.Abs(vehicleController.AccelInput); // get values for fading the sounds based on the acceleration
            decFade = 1 - accFade;                
            highFade = Mathf.InverseLerp(0.2f, 0.8f, vehicleController.RevsAudio); // get the high fade value based on the cars revs
            lowFade = 1 - highFade;                
            highFade = Mathf.Clamp ( 1 - ((1 - highFade)*(1 - highFade)), 0, volumeLimit); // adjust the values to be more realistic
            lowFade = Mathf.Clamp ( 1 - ((1 - lowFade)*(1 - lowFade)), 0, volumeLimit);
            accFade = Mathf.Clamp ( 1 - ((1 - accFade)*(1 - accFade)), 0, volumeLimit);
            decFade = Mathf.Clamp ( 1 - ((1 - decFade)*(1 - decFade)), 0, volumeLimit);                
            lowAccelerationSource.volume = lowFade*accFade; // adjust the source volumes based on the fade values
            lowDecelerationSource.volume = lowFade*decFade;
            highAccelerationSource.volume = highFade*accFade;
            highDecelerationSource.volume = highFade*decFade;
            if (vehicleController.wheelSettings.enableSkid) CheckForWheelSkid();
        }


        WheelHit wheelHit; // cached to prevent runtime gc
        Transform skidPrefab;
        bool useDefaultSkidPrefab;
        
        private void CheckForWheelSkid() // checks if the wheels are spinning and if so does three things 1) emits particles 2) plays tiure skidding sounds 3) leaves skidmarks on the ground
        {            
            for (int i = 0; i < 4; i++)
            {
                vehicleController.wheels [i].collider.GetGroundHit(out wheelHit);
                
                
                    if (vehicleController.wheels[i].collider.isGrounded && (Mathf.Abs(wheelHit.forwardSlip) >= vehicleController.wheelSettings.slipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= vehicleController.wheelSettings.slipLimit))  // is the tire slipping above the given threshhold
                    {
                        if (wheelHit.collider != null)
                        {
                            useDefaultSkidPrefab = true;
                            //Debug.Log(wheelHit.collider.material.name);
                            for (int j = 0; j < vehicleController.wheelSettings.skidTrailProfiles.Count; j++)
                            {
                                if (wheelHit.collider.material.name.Contains(vehicleController.wheelSettings.skidTrailProfiles[j].name))
                                {
                                    useDefaultSkidPrefab = false;
                                    skidPrefab = vehicleController.wheelSettings.skidTrailProfiles[j].SkidTrailPrefab;
                                    ParticleSystem.ColorOverLifetimeModule cm = vehicleController.wheels[i].wheelSmokeParticleSystem.colorOverLifetime;
                                    cm.color = vehicleController.wheelSettings.skidTrailProfiles[j].smokeColor;
                                    break;
                                }
                            }
                            if (useDefaultSkidPrefab) skidPrefab = vehicleController.wheelSettings.skidTrailProfiles[0].SkidTrailPrefab;
                        }

                        vehicleController.wheels[i].wheel.WheelSkid
                        (
                            vehicleController,
                            vehicleController.wheels[i].collider,
                            skidPrefab, 
                            vehicleController.wheels[i].wheelSmokeParticleSystem,
                            1,
                            vehicleController.wheels[i].audioSource,
                            this
                        );
                    }
                //}
                else
                {
                     vehicleController.wheels[i].wheel.WheelSkid_Stop
                    (
                        vehicleController.wheels[i].audioSource,
                        this,
                        vehicleController.wheels[i].wheelSmokeParticleSystem
                    );
                }
            }
        }
		
	}
}