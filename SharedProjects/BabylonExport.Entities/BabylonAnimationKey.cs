using System;
using System.Runtime.Serialization;

using Utilities;

namespace BabylonExport.Entities
{
    [DataContract]
    public class BabylonAnimationKey : IComparable<BabylonAnimationKey>, ICloneable
    {
        [DataMember]
        public float frame { get; set; }

        [DataMember]
        public float[] values { get; set; }

        public object Clone()
        {
            return new BabylonAnimationKey
            {
                frame = frame,
                values = (float[])values.Clone()
            };
        }

        public int CompareTo(BabylonAnimationKey other)
        {
            if (other == null)
                return 1;
            else
                return this.frame.CompareTo(other.frame);
        }

        public static float[] InterpolateBetweenTransScaleKeyframes(BabylonAnimationKey from, BabylonAnimationKey to, float amount, BabylonAnimation.AnimationInterpolationMode interpolationMode)
        {
            switch (interpolationMode)
            {
                case BabylonAnimation.AnimationInterpolationMode.Linear:
                default:
                    return MathUtilities.Lerp(from.values, to.values, amount);
            }
        }

        public static float[] InterpolateBetweenRotationQuaternionKeyframes(BabylonAnimationKey from, BabylonAnimationKey to, float amount, BabylonAnimation.AnimationInterpolationMode interpolationMode)
        {
            switch (interpolationMode)
            {
                case BabylonAnimation.AnimationInterpolationMode.Linear:
                default:
                    return BabylonQuaternion.Slerp(BabylonQuaternion.FromArray(from.values), BabylonQuaternion.FromArray(to.values), amount).ToArray();
            }
        }

        public static float[] InterpolateBetweenEulerRotationalKeyframes(BabylonAnimationKey from, BabylonAnimationKey to, float frame, BabylonAnimation.AnimationInterpolationMode interpolationMode)
        {
            float frameDiffNormalized = MathUtilities.GetLerpFactor(from.frame, to.frame, frame);

            switch (interpolationMode)
            {
                case BabylonAnimation.AnimationInterpolationMode.Linear:
                default:
                    return MathUtilities.LerpEulerAngle(from.values, to.values, frameDiffNormalized);
            }
        }
    }
}
