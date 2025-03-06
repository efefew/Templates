using UnityEngine;

namespace Project.Templates.Scripts.Camera
{
    public class ThirdPersonCamera : CameraOperator
    {
        public override void StartBootstrap() => base.StartBootstrap();
        protected override void CameraUpdate() => throw new System.NotImplementedException();
        protected override void Move(Vector2 position) => throw new System.NotImplementedException();
        protected override void ResetCamera() => throw new System.NotImplementedException();
        protected override void Rotate(float scaleRotation) => throw new System.NotImplementedException();
        protected override void Zoom(float scaleZoom) => throw new System.NotImplementedException();
    }
}
