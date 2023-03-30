// Copyright (c) 2022 Blobcreate & Ge Ge

using UnityEngine;

namespace Blobcreate.ProjectileToolkit
{
	public static class Projectile
	{
		/// <summary>
		/// Computes the launch velocity by the given start point, end point, and coefficient
		/// a of the quadratic function f(x) = ax^2 + bx + c which determines the trajectory
		/// of the projectile motion.
		/// </summary>
		/// <param name="a">The a coefficient of the quadratic function f(x) = ax^2 + bx + c.
		/// It determines the shape and speed of the trajectory, for example, -0.2f makes the
		/// trajectory curvier and slower while -0.01f makes it straighter and faster. Should
		/// always be negative.</param>
		public static Vector3 VelocityByA(Vector3 start, Vector3 end, float a)
		{
			var vec = end - start;
			var n = vec.y;
			vec.y = 0;
			var m = vec.magnitude;

			var b = n / m - m * a;
			var vx = Mathf.Sqrt(Physics.gravity.y / (2 * a)); // vy + g*m/vx = (2*a*m + b) * vx
			var vy = b * vx;
			var direction = vec / m;
			return new Vector3(direction.x * vx, vy, direction.z * vx);
		}

		/// <summary>
		/// Computes the launch velocity by the given start point, end point, and launch angle
		/// in degrees.
		/// </summary>
		/// <param name="elevationAngle">The launch angle in degrees. 0 means launch
		/// horizontally. Should be from -90f (exclusive) to 90f (exclusive) and greater than
		/// the elevation angle formed by start to end.</param>
		public static Vector3 VelocityByAngle(Vector3 start, Vector3 end, float elevationAngle)
		{
			var b = Mathf.Tan(Mathf.Deg2Rad * elevationAngle);

			var vec = end - start;
			var n = vec.y;
			vec.y = 0;
			var m = vec.magnitude;

			var a = (n / m - b) / m;
			var vx = Mathf.Sqrt(Physics.gravity.y / (2 * a));
			var vy = b * vx;
			var direction = vec / m;
			return new Vector3(direction.x * vx, vy, direction.z * vx);
		}

		/// <summary>
		/// Computes the launch velocity by the given start point, end point, and time in
		/// seconds the projectile flies from start to end. The projectile object will be
		/// exactly at the end point time seconds after launch.
		/// </summary>
		/// <param name="time">The time in seconds you want the projectile to fly from start
		/// to end.</param>
		public static Vector3 VelocityByTime(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(
				(end.x - start.x) / time,
				(end.y - start.y) / time - 0.5f * Physics.gravity.y * time,
				(end.z - start.z) / time);
		}

		/// <summary>
		/// Computes the launch velocity by the given start point, end point, and max height
		/// of the projectile motion.
		/// </summary>
		/// <param name="heightFromEnd">The height measured from the end point (for example,
		/// 1f means the max height of the trajectory is 1 meter above the end point). The
		/// algorithm automatically clamps the value if it is lower than the y value of
		/// start or end.</param>
		public static Vector3 VelocityByHeight(Vector3 start, Vector3 end, float heightFromEnd)
		{
			var h = end.y + heightFromEnd - start.y;
			if (h < 0)
			{
				h = 0;
				heightFromEnd = start.y - end.y;
			}

			var time = Mathf.Sqrt(-2 * h / Physics.gravity.y) + Mathf.Sqrt(-2 * heightFromEnd / Physics.gravity.y);
			return VelocityByTime(start, end, time);
		}

		/// <summary>
		/// Computes the two angle results by the given start point, end point, and launch
		/// speed. Returns false if out of reach.
		/// </summary>
		/// <param name="speed">The launch speed of the projectile object.</param>
		/// <param name="lowAngle">The lower angle that satisfies the conditions, or 0 if the
		/// method returns false.</param>
		/// <param name="highAngle">The higher angle that satisfies the conditions, or 0 if the
		/// method returns false.</param>
		public static bool AnglesBySpeed(Vector3 start, Vector3 end, float speed, out float lowAngle, out float highAngle)
		{
			var vec = end - start;
			var n = vec.y;
			vec.y = 0;
			var m = vec.magnitude;

			// Note that the b and c here are of the quadratic equation that calculates
			// the b value of the quadratic function of the projectile motion.
			var b = (2 * speed * speed) / (m * Physics.gravity.y);
			var c = -(b * n / m) + 1;
			var delta = b * b - 4 * c;

			if (delta < 0)
			{
				lowAngle = default;
				highAngle = default;
				return false;
			}

			var deltaRoot = Mathf.Sqrt(delta);
			lowAngle  = Mathf.Atan((-b - deltaRoot) * 0.5f) * Mathf.Rad2Deg;
			highAngle = Mathf.Atan((-b + deltaRoot) * 0.5f) * Mathf.Rad2Deg;
			return true;
		}

		/// <summary>
		/// Computes the two velocity results by the given start point, end point, and launch
		/// speed. Returns false if out of reach.
		/// </summary>
		/// <param name="speed">The launch speed of the projectile object.</param>
		/// <param name="lowAngleV">The lower-angle velocity that satisfies the conditions,
		/// or (0, 0, 0) if the method returns false.</param>
		/// <param name="highAngleV">The higher-angle velocity that satisfies the conditions,
		/// or (0, 0, 0) if the method returns false.</param>
		public static bool VelocitiesBySpeed(Vector3 start, Vector3 end, float speed, out Vector3 lowAngleV, out Vector3 highAngleV)
		{
			if (!AnglesBySpeed(start, end, speed, out var lowAngle, out var highAngle))
			{
				lowAngleV = default;
				highAngleV = default;
				return false;
			}

			var dirXZ = end - start;
			dirXZ.y = 0;
			dirXZ.Normalize();
			var right = Vector3.Cross(Vector3.down, dirXZ);

			var ro = Quaternion.AngleAxis(lowAngle, right);
			var dir = ro * dirXZ;
			lowAngleV = speed * dir;

			ro = Quaternion.AngleAxis(highAngle, right);
			dir = ro * dirXZ;
			highAngleV = speed * dir;

			return true;
		}

		/// <summary>
		/// Computes the position of the projectile at the given time counted from the moment
		/// the projectile is at origin.
		/// </summary>
		/// <param name="time">The time counted from the moment the projectile is at origin.</param>
		/// <param name="gAcceleration">Gravitational acceleration, equals the magnitude of
		/// gravity (normally equals Physics.gravity.y).</param>
		public static Vector3 PositionAtTime(Vector3 origin, Vector3 originVelocity, float time, float gAcceleration)
		{
			var vy = originVelocity.y + time * gAcceleration;
			var py = 0.5f * time * (originVelocity.y + vy);
			var displacement = new Vector3(time * originVelocity.x, py, time * originVelocity.z);
			return origin + displacement;
		}

		/// <summary>
		/// Computes the trajectory points of the projectile and stores them into the buffer.
		/// </summary>
		/// <param name="distance">To calculate the positions to how far, from origin and
		/// ignoring height.</param>
		/// <param name="count">How many positions to calculate, including the origin and end.</param>
		/// <param name="gAcceleration">Gravitational acceleration, equals the magnitude of
		/// gravity (normally equals Physics.gravity.y).</param>
		/// <param name="positions">The buffer to store the calculated positions.</param>
		public static void Positions(Vector3 origin, Vector3 originVelocity, float distance, int count, float gAcceleration, Vector3[] positions)
		{
			var vxz = originVelocity;
			vxz.y = 0;

			float timeInterval = distance / vxz.magnitude / (count - 1);
			var y = 0.5f * gAcceleration * timeInterval;
			positions[0] = origin;

			for (int i = 1; i < positions.Length; i++)
			{
				positions[i] = origin + i * timeInterval *
					new Vector3(originVelocity.x, originVelocity.y + i * y, originVelocity.z);
			}
		}

		// Will be available in the next release! PEB stands for physics-engine-based.
		// public static void PEBPositions(Vector3 origin, Vector3 originVelocity, int iterations, float gAcceleration, Vector3[] positions)
		// {
		// 	var vxz = originVelocity;
		// 	vxz.y = 0;

		// 	positions[0] = origin;
		// 	var prevPos = origin;
		// 	var vy = originVelocity.y;

		// 	for (int i = 1; i < positions.Length; i++)
		// 	{
		// 		vy += gAcceleration * Time.fixedDeltaTime;
		// 		positions[i] = prevPos + Time.fixedDeltaTime *
		// 			new Vector3(originVelocity.x, vy, originVelocity.z);
		// 		prevPos = positions[i];
		// 	}
		// }

		/// <summary>
		/// Tests if a projectile at start can use the vertical velocity (y) of startVelocity
		/// to hit the elevation (y) of end, if true, outputs the time of flight based on the
		/// vertical speed. Horizontal speed is ignored.
		/// </summary>
		/// <param name="startVelocity">The velocity at the start point, or launch velocity.</param>
		/// <param name="timesOfFlight">The time results that a projectile fly from start to
		/// end with the launch velocity startVelocity.</param>
		public static bool VerticalFlightTest(Vector3 start, Vector3 end, Vector3 startVelocity, out Vector2 timesOfFlight)
		{
			timesOfFlight = new Vector2(-1f, -1f);
			var a = 0.5f * Physics.gravity.y;
			var b = startVelocity.y;
			var c = start.y - end.y;

			var delta = b * b - 4 * a * c;
			if (delta < 0f)
				return false;

			var ta = (-b + Mathf.Sqrt(delta)) / (2 * a);
			var tb = (-b - Mathf.Sqrt(delta)) / (2 * a);
			timesOfFlight = new Vector2(ta, tb);
			return true;
		}

		/// <summary>
		/// Tests if a projectile at start can use startVelocity to hit end, and outputs the
		/// time of flight.
		/// </summary>
		/// <param name="startVelocity">The velocity at the start point, or launch velocity.</param>
		/// <param name="testMode">FlightTestMode (Enum).</param>
		/// <param name="timeOfFlight">The time that a projectile fly from start to end with
		/// the launch velocity startVelocity.</param>
		public static bool FlightTest(Vector3 start, Vector3 end, Vector3 startVelocity, FlightTestMode testMode, out float timeOfFlight)
		{
			var dXZ = end - start;
			var sqrDistance = dXZ.x * dXZ.x + dXZ.z * dXZ.z;
			var sqrSpeed = startVelocity.x * startVelocity.x + startVelocity.z * startVelocity.z;
			var testT = Mathf.Sqrt(sqrDistance / sqrSpeed);

			if (testMode == FlightTestMode.Horizontal)
			{
				timeOfFlight = testT;
				if (sqrSpeed == 0f)
				{
					if (sqrDistance == 0f)
						return true;
					else
						return false;
				}
				return true;
			}

			if (testMode == FlightTestMode.Both)
			{
				if (testT == float.NaN)
					testMode = FlightTestMode.VerticalB;
				else
				{
					var vy = startVelocity.y + testT * Physics.gravity.y;
					var py = 0.5f * testT * (startVelocity.y + vy) + start.y;
					timeOfFlight = testT;
					return Mathf.Abs(py - end.y) < 0.04f;
				}
			}

			if (testMode == FlightTestMode.VerticalB ||
				testMode == FlightTestMode.VerticalA)
			{
				if (VerticalFlightTest(start, end, startVelocity, out var results))
				{
					if (testMode == FlightTestMode.VerticalB)
						timeOfFlight = results.y;
					else
						timeOfFlight = results.x;

					return timeOfFlight >= 0f;
				}
			}

			timeOfFlight = -1f;
			return false;
		}

		/// <summary>
		/// Computes how far a projectile that uses the given speed at start can reach at the
		/// given elevation endElevation. Returns -1f if can't reach the elevation.
		/// </summary>
		/// <param name="endElevation">The elevation (y) of the target point you want the
		/// projectile motion to hit or pass through.</param>
		/// <param name="speed">The launch speed of the projectile object.</param>
		public static float ElevationalReach(Vector3 start, float endElevation, float speed)
		{
			var n = endElevation - start.y;
			var bm = (2 * speed * speed) / Physics.gravity.y;
			var invSqr = 4f / (bm * bm + 4f * bm * n);

			if (invSqr <= 0f)
				return -1f;

			return Mathf.Sqrt(1f / invSqr);
		}

		/// <summary>
		/// Computes how far a projectile that uses the given speed at start can reach at the
		/// given elevation endElevation, and outputs the corresponding launch angle. Returns
		/// -1f if can't reach the elevation.
		/// </summary>
		/// <param name="endElevation">The elevation (y) of the target point you want the
		/// projectile motion to hit or pass through.</param>
		/// <param name="speed">The launch speed of the projectile object.</param>
		/// <param name="angle">The angle that satisfies the conditions.</param>
		public static float ElevationalReach(Vector3 start, float endElevation, float speed, out float angle)
		{
			var n = endElevation - start.y;
			var bm = (2 * speed * speed) / Physics.gravity.y;
			var invSqr = 4f / (bm * bm + 4f * bm * n);

			if (invSqr <= 0f)
			{
				angle = default;
				return -1f;
			}

			var m = Mathf.Sqrt(1f / invSqr);
			var b = bm / m;
			angle = Mathf.Atan(-b * 0.5f) * Mathf.Rad2Deg;
			return m;
		}
	}

	public enum FlightTestMode
	{
		/// <summary>
		/// Calculates the time of flight based on the horizontal speed. Vertical speed is
		/// ignored.
		/// </summary>
		Horizontal,

		/// <summary>
		/// Calculates the time of flight that hit the elevation of the end point for the
		/// second time, based on the vertical speed. Horizontal speed is ignored.
		/// </summary>
		VerticalB,

		/// <summary>
		/// Calculates the time of flight that hit the elevation of the end point for the
		/// first time, based on the vertical speed. Horizontal speed is ignored.
		/// </summary>
		VerticalA,

		/// <summary>
		/// Tests the given velocity both horizontally and vertically, and outputs the time
		/// of flight if the velocity is correct.
		/// </summary>
		Both,

		//Strict // coming soon!
	}
}