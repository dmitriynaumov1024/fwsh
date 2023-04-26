<template>
<div class="width-container pad-1">
    <h2 class="margin-bottom-2">{{locale.index.title}}</h2>
    <router-link to="/people" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.person.plural}}</h3>
    </router-link>
    <router-link to="/resources" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.resource.plural}}</h3>
    </router-link>
    <router-link to="/blueprints" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.blueprint.plural}}</h3>
    </router-link>
    <router-link to="/orders" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.order.plural}}</h3>
    </router-link>
    <p class="margin-bottom-1"><b>Buttons</b></p>
    <div class="flex-stripe flex-spacing-1 margin-bottom-1">
        <button class="button button-primary">Primary</button>
        <button class="button button-secondary">Secondary</button>
        <button class="button button-inline">Inline</button>
        <span class="flex-grow"></span>
    </div>
    <div class="flex-stripe flex-spacing-1 margin-bottom-1">
        <button class="button button-primary accent-good">Good</button>
        <button class="button button-primary accent-warn">Warning</button>
        <button class="button button-primary accent-error">Error</button>
        <span class="flex-grow"></span>
    </div>
    <div class="flex-stripe flex-spacing-1">
        <button class="button button-secondary accent-good">Good</button>
        <button class="button button-secondary accent-warn">Warning</button>
        <button class="button button-secondary accent-error">Error</button>
        <span class="flex-grow"></span>
    </div>
    <div class="fancy-input">
        <label for="someid2">Name</label>
        <input type="text" pattern="[А-Яа-я\-]{2,16}" id="someid2" />
    </div>
    <div class="fancy-input">
        <label for="someid3">Password</label>
        <input type="password" id="someid3" />
    </div>
    <div class="fancy-textarea">
        <label for="someid4">Description</label>
        <textarea id="someid4"></textarea>
    </div>
    <div class="fancy-group">
        <header>Contact me via {{contactSelected || '?'}}</header>
        <main>
            <div class="fancy-radio" v-for="contact of contactVariants">
                <input type="radio" v-model="contactSelected" :value="contact" :id="'input_contact_'+contact" />
                <label :for="'input_contact_'+contact">{{contact}}</label>
            </div>
        </main>
    </div>
    <div class="fancy-group">
        <header>I am interested in</header>
        <main>
            <div class="fancy-checkbox" v-for="topic of topicVariants">
                <input type="checkbox" v-model="topicsSelected" :value="topic.key" :id="'input_topic_'+topic.key" />
                <label :for="'input_topic_'+topic.key">{{topic.name}}</label>
            </div>
        </main>
    </div>
</div>
</template>

<script setup>
import { ref, inject } from "vue"

const locale = inject("locale")

const contactSelected = ref(null)
const contactVariants = ["email", "phone"]

const topicsSelected = ref([])
const topicVariants = [
    { key: "cybersec", name: "Cybersecurity" },
    { key: "backend", name: "Backend development" },
    { key: "frontend", name: "Frontend development" }
]
</script>
