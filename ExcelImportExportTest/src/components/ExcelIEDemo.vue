<template>
    <div class="post">
        <div class="container px-10 py-10 mx-0 min-w-full flex flex-col items-center">
            <h1 class="font-bold text-2xl my-2"> Excel Import-Export Demo</h1>
            <label class="inline text-lg text-gray"> Path: </label>
            <input type="text" class="text-center my-2 bg-gray-50 border border-gray-300 text-gray-900 text-md rounded-lg focus:ring-blue-500 focus:border-blue-500 inline-block w-3/5 p-2.5" />
            <label class="inline text-lg text-gray"> Sheet: </label>
            <input type="text" class="text-center my-2 bg-gray-50 border border-gray-300 text-gray-900 text-md rounded-lg focus:ring-blue-500 focus:border-blue-500 inline-block w-3/5 p-2.5" />
            <button class="bg-blue-500 text-lg hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Generate</button>

            <button class="bg-green-500 text-lg hover:bg-green-700 text-white font-bold py-2 px-4 rounded" @click="awesome = !awesome">Toggle</button>


                <input v-model="filepath" class="text-center my-2 bg-gray-50 border border-gray-300 text-gray-900 text-md rounded-lg focus:ring-blue-500 focus:border-blue-500 inline-block w-3/5 p-2.5" type="text" name="filepath" />
                <input v-model="sheetname" class="text-center my-2 bg-gray-50 border border-gray-300 text-gray-900 text-md rounded-lg focus:ring-blue-500 focus:border-blue-500 inline-block w-3/5 p-2.5" type="text" name="sheetname" />
                <button @click="fetchSheetData(filepath, sheetname)" class="bg-blue-500 text-lg hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Generate</button>

                <form method="POST" enctype="multipart/form-data">
                    <label> Excel uploading form </label> <br />
                    <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Upload file</label>
                    <input id="files" accept=".xls, .xlsx" name="fileGetter" class="block w-full text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer bg-gray-50 dark:text-gray-400 focus:outline-none dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400" type="file">
                    <input type="button" v-on:click="uploadSheetData()" class="bg-blue-500 text-lg hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" value="Generate" />
                </form>

            <button class="bg-blue-500 text-lg hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" v-on:click="uploadJson()"> JSON Upload Test </button>

            <!-- Result -->
            <table v-if="sheetdata" class="w-3/5 text-sm text-left text-gray-500">
                <tbody>
                    <tr class="border-b border-gray-200" v-for="row in sheetdata">
                        <td class="border-b border-gray-200 " v-for="cell in row">
                            {{ cell }}
                        </td>
                    </tr>
                </tbody>
            </table>

        </div>
        
    </div>
</template>

<script lang="js">
    import $ from 'jquery';
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {
                sheetdata: false,

                //awesome: false,
                loading: false,
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchSheetData(path, sheetname) {
                fetch(`api/data/fromexcel/${encodeURIComponent(path)}/atsheet/${encodeURIComponent(sheetname)}`)
                    .then(r => r.text())
                    .then(text => {
                        if (!text.startsWith("ERROR")) {
                            this.sheetdata = text.split('\n');
                            for (var i = 0; i < this.sheetdata.length; i++) {
                                this.sheetdata[i] = this.sheetdata[i].split(' ');
                            }
                            console.log(this.sheetdata);
                        } else {
                            console.log('ERROR IGNORED!');
                        }
                        
                    });
            },

            uploadJson() {
                var t = {
                    "name": "Dung",
                    "age": 17,
                    "nicknames": ["Teo", "Chip"],
                    "classmate": {
                        "name": "Duy",
                        "age": 17,
                        "nicknames": ["Húi"]
                    }
                };

                var jsTxt = JSON.stringify(t);

                $.ajax({ // jQuery testing

                    type: "POST",

                    url: "/api/json/showimport",

                    contentType: "application/json; charset=utf-8",

                    processData: false,

                    data: {x : jsTxt},

                    async: false,

                    success: function (message) {

                        console.log(message);

                    },

                    error: function () {

                        alert("Error!");

                    },

                    complete: function () {



                    }

                });

            },

            uploadSheetData() {
                var fileUpload = document.getElementById('files');

                var files = fileUpload.files;

                var reader = new FileReader();


                var data = new FormData();

                data.append(files[0].name, files[0]);

                console.log(files[0].data);

                var current = this;

                $.ajax({ // jQuery testing

                    type: "POST",

                    url: "/api/excel/upload",

                    contentType: false,

                    processData: false,

                    data: data,

                    async: false,

                    success: function (message) {

                        console.log(message);

                    },

                    error: function () {

                        alert("Error!");

                    },

                    complete: function () {



                    }

                });
            },

            fetchData() {
                this.post = null;
                this.loading = true;

                fetch('api/data/hello')
                    .then(r => {
                        console.log(r);
                        return;
                    });
            }
        },
    });
</script>