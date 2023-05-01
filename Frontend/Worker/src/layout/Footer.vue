<template>
<footer class="footer">
    <div class="width-container pad-1-05">
        <p>{{locale.footer.siteName}} &ensp;
            <button class="button button-inline" @click="()=> beginSelectLocale()">
                {{locale.common.language}}
            </button>
        </p>
        <p>Copyright &copy; 2022 &ndash; {{currentYear}} Dmitriy Naumov</p>
        <p>Github: <a class="link" href="https://github.com/dmitriynaumov1024">dmitriynaumov1024</a></p>
    </div>
</footer>
<Modal v-if="isSelectingLocale">
    <h2 class="margin-bottom-05">{{locale.selectLanguage.title}}</h2>
    <div class="margin-bottom-2">
        <Radiobox v-for="localeOption of locales" class="margin-bottom-05"
            :checked="localeOption.displayName == locale.displayName"
            @click="()=> endSelectLocale(localeOption.key)">
            {{localeOption.displayName}}
        </Radiobox>
    </div>
    <div>
        <button class="button button-inline accent-gray" @click="()=> endSelectLocale()">
            {{locale.action.cancel}}
        </button>
    </div>
</Modal> 
</template>

<script setup>
import { ref, inject } from "vue"
import { useLocale } from "@common"
import Modal from "@/layout/Modal.vue"
import Radiobox from "@/comp/ctrl/Radiobox.vue"

const locale = inject("locale")

const currentYear = new Date().getFullYear()

const { locales, selectLocale } = useLocale()

const isSelectingLocale = ref(false)

function beginSelectLocale () {
    isSelectingLocale.value = true
}

function endSelectLocale (key = undefined) {
    isSelectingLocale.value = false
    if (key) selectLocale(key)
}

</script>