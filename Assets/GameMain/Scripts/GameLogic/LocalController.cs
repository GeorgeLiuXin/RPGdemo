﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Galaxy
{
	public class LocalController : ComponentBase
	{
		public override void OnComponentStart()
		{

		}

		public override void OnPreDestroy()
		{

		}

		void Update()
		{
			Vector3 vPosition;
			if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit info;
				if(Physics.Raycast(ray,out info))
				{
					if(info.collider.CompareTag("Ground"))
					{
						vPosition = info.point;
						vPosition.y = transform.position.y;
						ShowChilkEffect(info.point);
						Owner.MoveToPoint(vPosition);
					}
					else if(info.collider.CompareTag("Enemy"))
					{

					}
				}
			}
			if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit info;
				if(Physics.Raycast(ray, out info))
				{
					if(info.collider.CompareTag("Ground"))
					{
						vPosition = info.point;
						vPosition.y = transform.position.y;
						Owner.MoveToPoint(vPosition);
					}
				}
			}
		}
		
		void ShowChilkEffect(Vector3 hitPoint)
		{
			hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.2f, hitPoint.z);
			GameObject obj = GameObject.Instantiate(GameEntry.StaicGame.m_effectClick, hitPoint, Quaternion.identity);
			GameObject.Destroy(obj, 0.32f);
		}
	}
}
