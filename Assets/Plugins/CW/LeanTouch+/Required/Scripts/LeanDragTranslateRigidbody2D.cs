using UnityEngine;
using CW.Common;

namespace Lean.Touch
{
	/// <summary>This component allows you to translate the current Rigidbody2D GameObject.</summary>
	[RequireComponent(typeof(Rigidbody2D))]
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanDragTranslateRigidbody2D")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Translate Rigidbody2D")]
	public class LeanDragTranslateRigidbody2D : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The camera this component will calculate using.
		/// None/null = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] private float damping = 10.0f;

		[System.NonSerialized]
		private Rigidbody2D cachedRigidbody;

		private Camera cachedCamera;

		private bool targetSet;

		private Vector3 targetScreenPoint;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		protected virtual void OnEnable()
		{
			cachedRigidbody = GetComponent<Rigidbody2D>();
		}

		protected virtual void FixedUpdate()
		{
			// Make sure the camera exists and the targetScreenPoint is set
			if (cachedCamera != null && targetSet == true)
			{
				// Calculate required velocity to arrive in one FixedUpdate
				var oldPosition = transform.position;
				var newPosition = cachedCamera.ScreenToWorldPoint(targetScreenPoint);
				var velocity    = (newPosition - oldPosition) / Time.fixedDeltaTime;

				var factor = CwHelper.DampenFactor(damping, Time.fixedDeltaTime);

				// Apply the velocity
				#if UNITY_6000_0_OR_NEWER
					cachedRigidbody.linearVelocity = velocity * factor;
				#else
					cachedRigidbody.velocity = velocity * factor;
				#endif
			}
		}

		protected virtual void Update()
		{
			// Get the fingers we want to use
			var fingers = Use.UpdateAndGetFingers();

			// Make sure the camera exists
			cachedCamera = CwHelper.GetCamera(_camera, gameObject);

			if (cachedCamera != null)
			{
				if (fingers.Count > 0)
				{
					// If it's the first frame the fingers are down, grab the current screen point of this GameObject
					if (targetSet == false)
					{
						targetSet         = true;
						targetScreenPoint = cachedCamera.WorldToScreenPoint(transform.position);
					}

					// Shift target point based on finger deltas
					targetScreenPoint += (Vector3)LeanGesture.GetScreenDelta(fingers);
				}
				// Unset if no fingers are down
				else
				{
					targetSet = false;
				}
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
	using UnityEditor;
	using TARGET = LeanDragTranslateRigidbody2D;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class LeanDragTranslateRigidbody2D_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);
			
			Draw("Use");
			Draw("_camera", "The camera this component will calculate using.\n\nNone/null = MainCamera.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
		}
	}
}
#endif