import bpy
import json
import mathutils

def writeQuaternion(quat):
	return {"x" :quat[0], "y" : quat[1], "z": quat[2], "w": quat[3],}  

def writeVecXYZ(x, y, z):
	return {"x" :x, "y" : y, "z": z}  

def writeVec3(vertex):
	return {"x" :vertex[0], "y" : vertex[1], "z": vertex[2]}  

def writeUnityVec3(vertex):
    return {"x" : vertex[0], "y" : vertex[1] , "z" : vertex[2]}
#	return {"x" : -vertex[0], "y" : vertex[2] , "z" : vertex[1]}

def writeStrands(psys, world_matrix, settings):   
	piece = {"version": "2.0"}
	piece["scale"] = settings.scale
	piece["rotation"] = writeVec3(settings.rotation)
	piece["translation"] = writeVec3(settings.translation)
	piece["bothEndsImmovable"] = 1 if settings.bothEndsImmovable else 0
	piece["maxNumVerticesInStrands"] = settings.maxNumVerticesInStrand
	piece["numFollowHairsPerGuideHair"] = settings.numFollowHairsPerGuideHair
	piece["maxRadiusAroundGuideHair"] = settings.maxRadiusAroundGuideHair
	piece["numStrands"] = len(psys.particles)
	piece["isSorted"] = 1 if settings.isSorted else 0	

	piece["Materials"] = [{"name" : "test"}]				  

	i = 0
	
	strands = []
	for part in psys.particles:
		strand = {"index" : i}
		strand["numVerts"] = len(part.hair_keys)
		strand["texcoord"] = (0.000, 0.000)
		points = []
		for key in part.hair_keys:
			offset = world_matrix * mathutils.Vector(key.co)				   
			points.append(writeUnityVec3(offset))

		origin = mathutils.Vector(part.location)		 
		points.append(writeUnityVec3(origin))   
		
		strand["points"] = points	
		strands.append(strand)
		i += 1
	piece["Strands"] = strands  

	return piece

def writeMeshes(context, filepath, global_matrix):
	import os

	base_name, ext = os.path.splitext(filepath)
	context_name = [base_name, '', '', ext]  # Base name, scene name, frame number, extension
	scene = context.scene

	# Exit edit mode before exporting, so current object states are exported properly.
	if bpy.ops.object.mode_set.poll():
		bpy.ops.object.mode_set(mode='OBJECT')

	orig_frame = scene.frame_current

#   # Export an animation?
#   if EXPORT_ANIMATION:
#	scene_frames = range(scene.frame_start, scene.frame_end + 1)  # Up to and including the end frame.
#   else:
	scene_frames = [orig_frame]  # Dont export an animation.

	meshes = []
	materials = []

	# Loop through all frames in the scene and export.
	for frame in scene_frames:
#	if EXPORT_ANIMATION:  # Add frame to the filepath.
#		context_name[2] = '_%.6d' % frame

#	scene.frame_set(frame, 0.0)
#	if EXPORT_SEL_ONLY:
#		objects = context.selected_objects
#	else:
		objects = scene.objects

		full_path = ''.join(context_name)

		# Get all meshes
		for ob_main in objects:

			# ignore dupli children
			if ob_main.parent and ob_main.parent.dupli_type in {'VERTS', 'FACES'}:
				# XXX
				print(ob_main.name, 'is a dupli child - ignoring')
				continue

			obs = []
			if ob_main.dupli_type != 'NONE':
				# XXX
				print('creating dupli_list on', ob_main.name)
				ob_main.dupli_list_create(scene)

				obs = [(dob.object, dob.matrix) for dob in ob_main.dupli_list]

				# XXX debug print
				print(ob_main.name, 'has', len(obs), 'dupli children')
			else:
				obs = [(ob_main, ob_main.matrix_world)]

			for ob, ob_mat in obs:
				uv_unique_count = no_unique_count = 0
		
				output, mats = writeMesh(ob, scene, ob_mat, global_matrix)
				if output is not None:
				    meshes.append(output)
				    materials += mats
                
        
	scene.frame_set(orig_frame, 0.0)	
	
	return (meshes, materials)

def mesh_triangulate(me):
    import bmesh
    bm = bmesh.new()
    bm.from_mesh(me)
    bmesh.ops.triangulate(bm, faces=bm.faces)
    bm.to_mesh(me)
    bm.free()

def writeMesh(ob, scene, local_matrix, global_matrix):
	try:
		me = ob.to_mesh(scene, True, 'PREVIEW', calc_tessface=False)
	except RuntimeError:
		return [None, None]
	me.transform(global_matrix * local_matrix)
	mesh_triangulate(me)
	meshJson = {"name" : ob.name, "skeleton" : ob.parent.name if ob.parent is not None and ob.parent.type == 'ARMATURE' else ""}
	meshJson["AABB"] = { "min" : writeVecXYZ(-1, -1, 1), "max" : writeVecXYZ(1,1,1)} 
	vertexData = []
	for v in me.vertices:
		vertexData.append(writeUnityVec3(v.co))    
	meshJson["VertexBuffer"] = { "count" : len(vertexData), "types" : [("fff","position")], "data" : vertexData} 
	meshSections = {}
	for p in me.polygons:	
		key = p.material_index
		section = None
		if key in meshSections:
			section = meshSections[key]
		else:                           
			section = {"indexBuffer" : []}
			if len(me.materials) > 0:
				section["material"] = me.materials[key].name
			meshSections[key] = section
		section["indexBuffer"] += list(p.vertices)
	output = list(meshSections.values())
	meshJson["submesh_count"] = len(output)
	mats = []
	meshJson["MeshSections"] = output
	for mat in me.materials:
		mats.append({"name": mat.name })  
	# clean up
	bpy.data.meshes.remove(me)
	return [meshJson, mats]

def write_skeleton(context, filepath, global_matrix):
    # Exit edit mode before exporting, so current object states are exported properly.
    if bpy.ops.object.mode_set.poll():
        bpy.ops.object.mode_set(mode='EDIT')
    
    ob = bpy.context.object
    
    result = []
    
    if ob.parent is not None and ob.parent.type == 'ARMATURE':
        for bone in ob.parent.data.bones:
            output = {"name": bone.name}
            if bone.parent is not None:
                output["parent"] = bone.parent.name
            output["orientation"] = writeQuaternion(bone.matrix_local.to_quaternion())
            output["position"] = writeVec3(bone.matrix_local.to_translation())
            output["scale"] = writeVec3(bone.matrix_local.to_scale())
            result.append(output)             

    bpy.ops.object.mode_set(mode='OBJECT')

    return [{"Bones" : result}]

def write_some_data(context, filepath, settings):
	print("running write_some_data...")
	f = open(filepath, 'w', encoding='utf-8')   
	meshSystem = { "asset_name" : "name", "assetInfo": 1, "meshSystemVersion" : 1, "meshSystemAssetVersion" : 1}  
	meshSystem["AABB"] = { "min" : writeVecXYZ(-1, -1, 1), "max" : writeVecXYZ(1,1,1)}
	meshSystem["Skeletons"] = write_skeleton(context, filepath, settings.global_matrix)
	meshSystem["Animations"] = [{"AnimationTracks" : []}]

# Write out meshes
	
#   mesh = {"name" : "test", "skeleton" : "", "submesh_count" : 5, }
#   mesh["AABB"] = { "min" : (-1, -1, 1), "max" : (1,1,1)} 
#   mesh["vertexbuffer"] = { "count" : 2, "types" : [], "data" : []}
#   mesh["MeshSections"] = [{"material" : "", "indexbuffer" : [(0,1,2)]}, {"material" : "", "indexbuffer" : [(0,1,2)]} ]
	
	meshes, materials = writeMeshes(context, filepath, settings.global_matrix)
	meshSystem["Meshes"] = meshes
	meshSystem["Materials"] = materials

# Write out TressFX particle system
	ob = bpy.context.object
	psys = ob.particle_systems.active  
	world_matrix = mathutils.Matrix(ob.matrix_world)
	
	if psys is not None:
		pieces = []
		pieces.append(writeStrands(psys, world_matrix, settings))
		meshSystem["TressFX"] = pieces  
	
	json.dump(meshSystem, f, indent="\t", sort_keys=True)
	f.close()

	return {'FINISHED'}


# ExportHelper is a helper class, defines filename and
# invoke() function which calls the file selector.
from bpy_extras.io_utils import ExportHelper
from bpy.props import StringProperty, BoolProperty, EnumProperty, FloatProperty, FloatVectorProperty, IntProperty
from bpy.types import Operator


class ExportEZMesh(Operator, ExportHelper):
	"""This appears in the tooltip of the operator and in the generated docs"""
	bl_idname = "export_test.ezmesh"  # important since its how bpy.ops.import_test.some_data is constructed
	bl_label = "Export (.json)"

	# ExportHelper mixin class uses this
	filename_ext = ".json"

	filter_glob = StringProperty(
			default="*.json",
			options={'HIDDEN'},
			)

	# List of operator properties, the attributes will be assigned
	# to the class instance from the operator settings before calling.
	scale = FloatProperty(name="Scale", description="TressFX Scale", default=1.0, min=0.0,)

	rotation = FloatVectorProperty(name="rotation", description="Rotation", default=(0.0, 0.0, 0.0), size=3)

	translation = FloatVectorProperty(name="translation", description="Translation", default=(0.0, 0.0, 0.0), size=3)

	bothEndsImmovable = BoolProperty(name="bothEndsImmovable", description="Both Ends Immovable", default=False)

	maxNumVerticesInStrand = IntProperty(name="maxNumVerticesInStrand", description="Maximum Number of Vertices In Strand", default=16, min = 0)

	numFollowHairsPerGuideHair = IntProperty(name="numFollowHairsPerGuideHair", description="Number of follow hairs per guide hair", default=4, min = 0)

	maxRadiusAroundGuideHair = FloatProperty(name="maxRadiusAroundGuideHair", description="Maximum radius around guide hair", default=0.5, min = 0.0)  

	isSorted = BoolProperty(name="is Sorted", description="Is Sorted", default=True)
	
	axis_forward = EnumProperty(name="Forward",items=(('X', "X Forward", ""),('Y', "Y Forward", ""),('Z', "Z Forward", ""),('-X', "-X Forward", ""),('-Y', "-Y Forward", ""),('-Z', "-Z Forward", ""),),default='-Z',)
	  
	axis_up = EnumProperty(name="Up",items=(('X', "X Up", ""),('Y', "Y Up", ""),('Z', "Z Up", ""),('-X', "-X Up", ""),('-Y', "-Y Up", ""),('-Z', "-Z Up", ""),),default='Y',)
		
	global_scale = FloatProperty(name="Global Scale", description="Mesh Scale", min=0.01, max=1000.0, default=1.0,)
			
	def execute(self, context):
		from mathutils import Matrix
		from bpy_extras.io_utils import axis_conversion
								 
		
		self.global_matrix = (Matrix.Scale(self.global_scale, 4) *
						 axis_conversion(to_forward=self.axis_forward,
										 to_up=self.axis_up,
										 ).to_4x4())	
	
		return write_some_data(context, self.filepath, self)


# Only needed if you want to add into a dynamic menu
def menu_func_export(self, context):
	self.layout.operator(ExportEZMesh.bl_idname, text="EZMesh & TressFX (.json)")


def register():
	bpy.utils.register_class(ExportEZMesh)
	bpy.types.INFO_MT_file_export.append(menu_func_export)


def unregister():
	bpy.utils.unregister_class(ExportEZMesh)
	bpy.types.INFO_MT_file_export.remove(menu_func_export)


if __name__ == "__main__":
	register()

	# test call
	bpy.ops.export_test.ezmesh('INVOKE_DEFAULT')
