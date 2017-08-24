var filesUsed = [];
var idUsed=[];
var json={};
var counter=0;
var key;
	
$(document).ready(function(e){
	var input = document.getElementById('files');
	if(input == null)
	{
		ReadDataFromFireBase();
	}
});

$("#btnSave").click(function(event){
	
	var playersRef = firebase.database().ref("documents/");
    playersRef.push({
	docId:$("#imgId").attr('src').replace(/\/.*\//,''),
	ocr:$("#ocr").text()
		});

     firebase.database().ref("docBrut/"+key).update({
     	state:"1"
     });
     ReadDataFromFireBase();
		//HandleFileSelect();
});

function ReadDataFromFireBase()
{
	 docBruteRef = firebase.database().ref("docBrut/")
	.orderByChild("state").equalTo("0").limitToFirst(1);


	docBruteRef.on("child_added",function(data){
		$("#ocr").text(data.val().ocr);
		$("#imgId").attr('src',data.val().docId);
		key=data.key;
		if($("#ocr").text() =="")
		{
			alert("Aucun lot disponible");
		}

	});
}

function ocearizeAll()
{
	var input = document.getElementById('files');
    	var  files = input.files;
		
		for(var i=0,ftext;ftext= files[i];i++)
		{
			if(!ftext.type.match('image.*'))
			{
				continue;
				
			}
		var ocrb=	Ocearize(ftext);
		var docBrute = firebase.database().ref("documents/");
        docBrute.push({

		docId:ftext.name,
		ocrbrute:ocrb,
		});

		}
}
function loadImageName(event)
{
	var files = event.target.files;//files list object
		
		
		for(var i=0,f;f= files[i];i++)
		{

			if( !f.type.match('image.*'))
			{ 
				continue;
			}
			filesUsed.push(f.name);
			
	    }
	    HandleFileSelect();
	    HandleFileText();
	  
}	

	
	function HandleFileText()
	{
		var input = document.getElementById('files');
    	var  files = input.files;
		var flag = false;
		for(var i=0,ftext;ftext= files[i];i++)
		{
			if(flag)
			{
				break;
			}
			if(!ftext.type.match('text.*'))
			{
				if(ftext.name.replace(/\..{1,4}/,'') !== $("#imgId").attr('title').replace(/\..{1,4}/,''))
				{
					continue;
				}
				
			}
			else{
				flag=true;
			}
			
			var reader = new FileReader();
			
			reader.onload=(function(theFile){
				return function(e){
					
					$("#ocr").text(e.target.result);
			
				}
			})(ftext);
			
			reader.readAsText(ftext);
			
		}
	}
	function HandleFileSelect()
	{
		var input = document.getElementById('files');
    	var  files = input.files;
		
		var flag = false;
		
		for(var i=0,f;f= files[i];i++)
		{
			if(flag)
				break;

			if( !f.type.match('image.*')  )
			{ 
				continue;
			}
			else{
				if($.inArray(i,idUsed)!= -1)
				{
					continue;
				}
				idUsed.push(i);
			}
			Ocearize(f);
			var reader = new FileReader();
			
			
			reader.readAsDataURL(f);
			reader.onload=(function(theFile){
				return function(e){
					
					$("#imgId").attr('src',e.target.result);
					
					$("#imgId").attr('title',escape(theFile.name));
				}

			})(f);
			
			
			flag=true;
		
	    }

	    }
	    function uploadFile()
	    {
	    	$('#list').text("Processing .... Please wait");
	    	var input = document.getElementById('files');
    	    var  files = input.files;

			var formData = new	FormData();
			for(var i=0,f;f= files[i];i++)
			{
				if(!f.type.match("image.*"))
				{
					continue;
				}
				formData.append('file'+i,f,f.name);
			}
			$.ajax({

				type:"POST",
				contentType:false,
				processData:false,
				url:'/umbraco/Surface/Home/uploadFile',
				data:formData,
				success:function(data){
					$('#list').text(data);
				},
				
			});
	    }
	    
	if(document.getElementById('files') != null)
	{
		document.getElementById('files').addEventListener('change', uploadFile, false);
	}
	 
	