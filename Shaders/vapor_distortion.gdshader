shader_type canvas_item;
render_mode blend_mix;

uniform vec4 albedo : source_color;
uniform sampler2D existing_screen_texture: source_color, hint_screen_texture, filter_linear_mipmap, repeat_enable;

uniform sampler2D noise: source_color, filter_linear_mipmap, repeat_enable;
uniform float distortion_intensity = 0.1;

uniform vec2 distortion_tiling = vec2(1.0, 1.0);
uniform vec2 distortion_offset_speed;

void fragment() {
	vec2 base_uv = SCREEN_UV;
	
	vec2 noise_uv = base_uv * distortion_tiling + distortion_offset_speed * TIME;
	float noise_tex = texture(noise, noise_uv).r;
	
	vec2 distortion_uv = base_uv + noise_tex * distortion_intensity;
	
	vec4 albedo_tex = texture(existing_screen_texture, distortion_uv);
	albedo_tex *= COLOR;
	COLOR = albedo * albedo_tex;
}