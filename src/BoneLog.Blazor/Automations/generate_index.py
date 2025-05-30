from dataclasses import dataclass
from typing import List, Optional
import os
import yaml
import json
import re

@dataclass
class PostHeader:
    title: str
    date: str
    tags: List[str]
    thumbnail: Optional[str] = None
    shortDescription: Optional[str] = None
    filename: Optional[str] = None

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
POSTS_PATH = os.path.join(BASE_DIR, '../wwwroot/data/posts')
OUTPUT_PATH = os.path.join(BASE_DIR, '../wwwroot/data/posts.json')

all_posts = []

for filename in os.listdir(POSTS_PATH):
    if not filename.endswith('.md'):
        continue

    full_path = os.path.join(POSTS_PATH, filename)
    with open(full_path, 'r', encoding='utf-8-sig') as f:
        content = f.read()

    if content.lstrip().startswith('---'):
        parts = content.split('---', 2)
        if len(parts) >= 3:
            yaml_text = parts[1].strip()
            try:
                data = yaml.safe_load(yaml_text)
                data.pop('cover', None)
                data['filename'] = os.path.splitext(filename)[0]
                post = PostHeader(**data)
                all_posts.append(post)
            except Exception as e:
                print(f"Error parsing YAML in {filename}: {e}")
        else:
            print(f"Invalid front matter format in {filename}")
    else:
        print(f"No front matter found in {filename}")

os.makedirs(os.path.dirname(OUTPUT_PATH), exist_ok=True)

with open(OUTPUT_PATH, 'w', encoding='utf-8') as f:
    json.dump([post.__dict__ for post in all_posts], f, ensure_ascii=False, indent=2)

print("posts.json generated.")
