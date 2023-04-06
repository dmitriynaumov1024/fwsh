<template>
<footer class="footer">
    <div class="width-container pad-1">
        <p>Furniture workshop 
            <span v-for="locale in locales" 
                @click="() => selectLocale(locale.key)">
                {{locale.displayName}}&ensp;
            </span>
        </p>
        <p>Copyright &copy; 2022 &ndash; 2023 Dmitriy Naumov</p>
        <p>Github: <a class="link" href="https://github.com/dmitriynaumov1024">dmitriynaumov1024</a></p>
    </div>
</footer>
</template>

<script setup>
import { useLocale } from "Common"

const { locales, selectLocale } = useLocale()

</script>