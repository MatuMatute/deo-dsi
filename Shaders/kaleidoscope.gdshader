shader_type canvas_item;

uniform vec2 resolution = vec2(960.0, 540.0);
uniform sampler2D existing_screen_texture: source_color, hint_screen_texture, filter_linear_mipmap, repeat_enable;
uniform float slices: hint_range(2.0, 100.0, 1.0) = 12.0;
uniform float speed: hint_range(0.0, 10.0, 0.01) = 0.5;

mat2 rotate(float a) {
	float sa = sin(a);
	float ca = cos(a);
	return mat2(vec2(ca, sa), vec2(-sa, ca));
}

vec2 fold(vec2 uv) {
	float theta = atan(uv.x, uv.y);
	float slice = TAU / slices;
	float id = floor(theta / slice);
	float angle = slice * id;
	uv = rotate(angle) * uv;
	
	if (mod(id, 2.0) >= 1.0) {
		uv.y = -uv.y;
		angle = -(slice * (id + slices / 2.0 + 1.0));
	}
	
	return uv;
}

void fragment() {
	vec2 uv = UV - 0.5;
	uv.x *= resolution.x / resolution.y;
	float angle = TIME * speed;
	uv = rotate(angle) * fold(uv);
	COLOR = texture(existing_screen_texture, uv);
}