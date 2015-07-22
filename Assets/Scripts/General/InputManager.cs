using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InputManager : MonoBehaviour {

	static InputManager instance;

	private EventSystem eventSystem;

	private bool m_touchDown;
	private bool m_touchUp;

	private bool m_mouseDown;
	private bool m_mouseUp;

	private bool activeTouch;
	private int activeTouchId;

	private Vector3 m_cursorPos;

	private Vector3 m_deltaPosition;
	private bool	m_useTouch = false;

	void Start()
	{
		m_useTouch = Application.platform == RuntimePlatform.Android 
			|| Application.platform == RuntimePlatform.BlackBerryPlayer
				|| Application.platform == RuntimePlatform.IPhonePlayer;

		if(instance == null){
			instance = gameObject.GetComponent<InputManager>();
		}else{
			Destroy(gameObject);
		}

		eventSystem = EventSystem.current;
	}

	// Update is called once per frame
	void Update () {
		ProcessInput ();
	}

	void ProcessInput()
	{
		m_touchUp = false;
		m_touchDown = false;
		m_mouseDown = false;
		m_mouseUp = false;
		
		if (Input.GetMouseButtonDown (0)) {
			m_mouseDown = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			m_mouseUp = true;
		} 
		
		m_cursorPos = Input.mousePosition;
		
		if (!activeTouch) {
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					activeTouch = true;
					activeTouchId = touch.fingerId;

					m_touchDown = true;
				}
			}
		} else {
			
			Touch currTouch = Input.GetTouch(activeTouchId);
			
			m_cursorPos = currTouch.position ;
			
			if( currTouch.phase == TouchPhase.Ended )
			{
				activeTouch = false;
				m_touchUp = true;
			}
		}

		m_cursorPos.z = 0f;
	}

	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

	public static InputManager Instance{
		get {return instance;}
	}

	public bool UISelected{
		get {return (eventSystem.currentSelectedGameObject != null);} 
	}

	public bool UseTouch
	{
		get {return m_useTouch;}
	}

	public bool ActiveTouch
	{
		get {return activeTouch;}
	}

	public int ActiveTouchId
	{
		get {return activeTouchId;}
	}

	public bool ClickDown
	{
		get {return m_touchDown || m_mouseDown;}
	}

	public bool ClickUp
	{
		get {return m_touchUp || m_mouseUp;}
	}

	public Vector3 CursorPosition
	{
		get {return m_cursorPos;}
	}

	public Vector3 WorldCursorPosition
	{
		get 
		{ 
			Vector3 rtrn = Camera.main.ScreenToWorldPoint( m_cursorPos ); 
			rtrn.z = 0f;
			return rtrn;
		}
	}
}
