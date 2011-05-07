using UnityEngine;
using System.Collections;


public class UIObject : System.Object
{
	public delegate void UIObjectTransormChagedDelegate();
	public event UIObjectTransormChagedDelegate onTransformChanged;
	
	private GameObject _client; // Reference to the client GameObject
    public GameObject client
    {
    	get { return _client; }
    }
    protected Transform clientTransform; // Cached Transform of the client GameObject
    private UIObject _parentUIObject;
    public virtual Color color { get; set; } // hack that is overridden in UISprite just for animation support
    
    
	/// <summary>
	/// Sets up the client GameObject along with it's layer and caches the transform
	/// </summary>
    public UIObject()
    {
		// Setup our GO
		_client = new GameObject( this.GetType().Name );
		_client.transform.parent = UI.instance.transform; // Just for orginization in the hierarchy
		_client.layer = UI.instance.layer; // Set the proper layer so we only render on the UI camera
		
		// Cache the clientTransform
		clientTransform = _client.transform;
    }

	
	#region Transform passthrough properties so we can update necessary verts when changes occur
	
	public float zIndex
	{
		get { return clientTransform.position.z; }
		set
		{
			var pos = clientTransform.position;
			pos.z = value;
			clientTransform.position = pos;
		}
	}

	
	public virtual Vector3 position
	{
		get { return clientTransform.position; }
		set
		{
			clientTransform.position = value;
			if( onTransformChanged != null )
				onTransformChanged();
		}
	}


	public virtual Vector3 localPosition
	{
		get { return clientTransform.localPosition; }
		set
		{
			clientTransform.localPosition = value;
			if( onTransformChanged != null )
				onTransformChanged();
		}
	}
	
	
	public virtual Vector3 eulerAngles
	{
		get { return clientTransform.eulerAngles; }
		set
		{
			clientTransform.eulerAngles = value;
			if( onTransformChanged != null )
				onTransformChanged();
		}
	}


	public virtual Vector3 localScale
	{
		get { return clientTransform.localScale; }
		set
		{
			clientTransform.localScale = value;
			if( onTransformChanged != null )
				onTransformChanged();
		}
	}
	
	
	public virtual Transform parent
	{
		get { return clientTransform.parent; }
	}
	

	/// <summary>
	/// Setting the parentUIObject automatically sets up a listener for changes to the tranform.
	/// When the parent transform changes, UISprite's will automatically call updateTransform to keep their
	/// touch frames and actual positions in sync with the parent
	/// </summary>
	public UIObject parentUIObject
	{
		get { return _parentUIObject; }
		set
		{
			if( value == _parentUIObject )
				return;
			
			// remove the old listener if we have one
			if( _parentUIObject != null )
				_parentUIObject.onTransformChanged -= transformChanged;
			
			_parentUIObject = value;
			clientTransform.parent = _parentUIObject.clientTransform;
			
			// add the new listener
			_parentUIObject.onTransformChanged += transformChanged;
		}
	}
	
	#endregion
	
	
	public virtual void transformChanged()
	{

	}

}
